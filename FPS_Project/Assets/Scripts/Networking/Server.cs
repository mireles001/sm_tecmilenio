using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkCore
{
  public override void Init(ManagerUI ui)
  {
    base.Init(ui);

    _isServer = true;

    ConnectionConfig cc = new ConnectionConfig();
    _reliableChannel = cc.AddChannel(QosType.Reliable);
    _unreliableChannel = cc.AddChannel(QosType.Unreliable);

    HostTopology topo = new HostTopology(cc, MAX_USERS);
    _hostId = NetworkTransport.AddHost(topo, PORT, null);

    _ui.ConsoleMsg(string.Format("Opening connection on port {0}", PORT));
  }

  public override void UpdateMessagePump()
  {
    if (!_isStarted)
    {
      return;
    }

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
        _ui.ConsoleMsg(string.Format("User {0} has connected!", connectionId));
        Net_ConnectionId cnnId = new Net_ConnectionId();
        cnnId.ConnectionId = connectionId;
        SendClient(connectionId, cnnId);
        break;

      case NetworkEventType.DisconnectEvent:
        _ui.ConsoleMsg(string.Format("User {0} has disconnected :(", connectionId));
        break;

      case NetworkEventType.DataEvent:
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(recBuffer);
        NetMsg msg = (NetMsg)formatter.Deserialize(ms);

        OnData(connectionId, channelId, recHostId, msg);
        break;

      default:
      case NetworkEventType.BroadcastEvent:
        _ui.ConsoleMsg("Unexpected network event type");
        break;
    }
  }

  private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
  {
    switch (msg.OP)
    {
      case NetOP.None:
        _ui.ConsoleMsg("Unexpected NETOP");
        break;

      case NetOP.SetUsername:
        SetUsername(cnnId, channelId, recHostId, (Net_SetUsername)msg);
        break;
    }
  }

  private void SetUsername(int cnnId, int channelId, int recHostId, Net_SetUsername su)
  {
    _ui.ConsoleMsg(string.Format("Set username: {0}", su.Username));
  }
}
