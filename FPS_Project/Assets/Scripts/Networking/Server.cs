using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkCore
{
  private int _currentNumberOfData = 0;
  private float _currentUpdateTime = 0f;

  public override void Init(GameMaster master)
  {
    base.Init(master);

    ConnectionConfig cc = new ConnectionConfig();
    _reliableChannel = cc.AddChannel(QosType.Reliable);
    _unreliableChannel = cc.AddChannel(QosType.Unreliable);
    _stateUpdateChannel = cc.AddChannel(QosType.StateUpdate);

    HostTopology topo = new HostTopology(cc, MAX_USERS);
    _hostId = NetworkTransport.AddHost(topo, PORT, null);

    _master.ConsoleMsg(string.Format("Opening connection on port {0}", PORT));

    /*GameState.GetInstance().Init();
    PlayerInstance p = new PlayerInstance(_username, 0);
    GameState.GetInstance().localPlayer = p;
    GameState.GetInstance().AddPlayer(p);*/
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
        _master.ConsoleMsg(string.Format("User {0} has connected!", connectionId));
        Net_ConnectionId cnnId = new Net_ConnectionId();
        cnnId.ConnectionId = connectionId;
        GameState.GetInstance().AddPlayer(new PlayerInstance(_username, connectionId));
        SendClient(connectionId, cnnId, _reliableChannel);
        break;

      case NetworkEventType.DisconnectEvent:
        _master.ConsoleMsg(string.Format("User {0} has disconnected :(", connectionId));
        GameState.GetInstance().RemovePlayer();
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
    if (_currentUpdateTime >= SERVER_UPDATE_TIME_MS)
    {
      _currentUpdateTime -= SERVER_UPDATE_TIME_MS;
      UpdateGameState();
    }
  }

  public virtual void UpdateGameState()
  {
    if (!_isStarted)
      return;
    var pls = GameState.GetInstance().GetPlayerList();
    Net_GameState gs = new Net_GameState();
    gs.players = pls;
    LameBroadCast(gs, _stateUpdateChannel);
  }

  private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
  {
    switch (msg.OP)
    {
      case NetOP.None:
        _master.ConsoleMsg("Unexpected NETOP");
        break;

      case NetOP.SetUsername:
        SetUsername(cnnId, channelId, recHostId, (Net_SetUsername)msg);
        break;

      case NetOP.PlayerPushUpdate:
        UpdatePlayer(cnnId, channelId, recHostId, (Net_PlayerPushUpdate)msg);
        break;
    }
  }

  private void SetUsername(int cnnId, int channelId, int recHostId, Net_SetUsername su)
  {
    _master.ConsoleMsg(string.Format("Set username: {0}", su.Username));
  }

  private void UpdatePlayer(int cnnId, int channelId, int recHostId, Net_PlayerPushUpdate playerDef)
  {
    Debug.Log("got update fron cnnid: " + cnnId + " and p.id " + playerDef.player.id);
    GameState.GetInstance().UpdatePlayer(
      playerDef.player
    );
  }
}
