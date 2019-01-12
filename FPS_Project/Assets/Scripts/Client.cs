using UnityEngine;
using UnityEngine.Networking;

public class Client : NetworkCore
{
  [SerializeField]
  private string _serverIp = "127.0.0.1";

  public override void Init()
  {
    base.Init();

    ConnectionConfig cc = new ConnectionConfig();
    _reliableChannel = cc.AddChannel(QosType.Reliable);

    HostTopology topo = new HostTopology(cc, MAX_USERS);
    _hostId = NetworkTransport.AddHost(topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
    // Web Client
    NetworkTransport.Connect(_hostId, _serverIp, WEB_PORT, 0, out _error);
    Debug.Log("Connecting from web");
#else
    // Standalone Client
    NetworkTransport.Connect(_hostId, _serverIp, PORT, 0, out _error);
    Debug.Log("Connecting from standalone");
#endif

    Debug.Log(string.Format("Attempting to connect on {0}", _serverIp));
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
        Debug.Log("We have connected to the server");
        break;

      case NetworkEventType.DisconnectEvent:
        Debug.Log("We have been disconnected");
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
