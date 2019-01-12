using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkCore
{
  private int _webHostId;

  public override void Init()
  {
    base.Init();

    ConnectionConfig cc = new ConnectionConfig();
    _reliableChannel = cc.AddChannel(QosType.Reliable);

    HostTopology topo = new HostTopology(cc, MAX_USERS);
    _hostId = NetworkTransport.AddHost(topo, PORT, null);
    _webHostId = NetworkTransport.AddWebsocketHost(topo, PORT, null);

    Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
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
        Debug.Log(string.Format("User {0} has connected!", connectionId));
        break;

      case NetworkEventType.DisconnectEvent:
        Debug.Log(string.Format("User {0} has disconnected :(", connectionId));
        break;

      case NetworkEventType.DataEvent:
        Debug.Log("Data");
        break;

      default:
      case NetworkEventType.BroadcastEvent:
        Debug.Log("Unexpected network event type");
        break;
    }
  }
}
