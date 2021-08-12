using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client : NetworkCore
{
  private int _limitAttemptsBefore = 10;
  protected float _currentUpdateTime = 0;
  protected float _currentNumberUpdates = 0;

  public override void Init(GameMaster master)
  {
    base.Init(master);

    ConnectionConfig cc = new ConnectionConfig();
    _reliableChannel = cc.AddChannel(QosType.Reliable);
    _unreliableChannel = cc.AddChannel(QosType.Unreliable);
    _stateUpdateChannel = cc.AddChannel(QosType.StateUpdate);

    HostTopology topo = new HostTopology(cc, MAX_USERS);
    _hostId = NetworkTransport.AddHost(topo, 0);

    _connectionId = NetworkTransport.Connect(_hostId, _serverIp, PORT, 0, out _error);
    _master.ConsoleMsg(string.Format("Attempting to connect on {0}", _serverIp));
  }

  public override void UpdateMessagePump()
  {
    if (!_isStarted)
      return;

    int recHostId;    // Is this from Web or Standalone?
    int connectionId; // Which user is sending me this?
    int channelId;    // Which lane is he sending that message from?

    byte[] recBuffer = new byte[BYTE_SIZE];
    int dataSize;

    NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out _error);

    switch (type)
    {
      case NetworkEventType.Nothing:
        break;

      case NetworkEventType.ConnectEvent:
        _master.ConsoleMsg("We have connected to the server.");
        GameState.GetInstance().localPlayer.id = connectionId;
        break;

      case NetworkEventType.DisconnectEvent:
        _master.Disconnect();
        break;

      case NetworkEventType.DataEvent:
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(recBuffer);
        NetMsg msg = (NetMsg)formatter.Deserialize(ms);

        OnData(connectionId, channelId, recHostId, msg);
        break;

      default:
      case NetworkEventType.BroadcastEvent:
        _master.ConsoleMsg("Unexpected network event type");
        break;
    }

    _currentUpdateTime += Time.deltaTime * 1000;
    if (_currentUpdateTime >= CLIENT_UPDATE_TIME_MS)
    {
      _currentUpdateTime -= CLIENT_UPDATE_TIME_MS;
      UpdateGameState();
    }

  }

  public void UpdateGameState()
  {
    if (!_isStarted)
      return;

    var p = GameState.GetInstance().localPlayer;
    Net_PlayerPushUpdate up = new Net_PlayerPushUpdate();
    up.player = p;
    _currentNumberUpdates++;
    SendServer(up, _stateUpdateChannel);

    /*if (_currentNumberUpdates > _limitAttemptsBefore)
    {
      _master.ConsoleMsg(string.Format("Shutdown after {0} attempts.", _limitAttemptsBefore));
      _master.Disconnect();
    }*/
  }

  private void UpdateGameStateOnData(int cnnId, int channelId, int recHostId, Net_GameState gs)
  {
    GameState.GetInstance().UpdateState(gs.players);
  }

  private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
  {
    switch (msg.OP)
    {
      case NetOP.None:
        _master.ConsoleMsg("Unexpected NETOP");
        break;

      case NetOP.SetConnectionId:
        SetConnectionId(cnnId, channelId, recHostId, (Net_ConnectionId)msg);
        break;
      case NetOP.GameState:
        UpdateGameStateOnData(cnnId, channelId, recHostId, (Net_GameState)msg);
        break;
    }
  }

  private void SetConnectionId(int cnnId, int channelId, int recHostId, Net_ConnectionId ci)
  {
    _master.ConsoleMsg(string.Format("Connection ID: {0}", ci.ConnectionId));
    GameState.GetInstance().localPlayer.id = ci.ConnectionId;
  }
}
