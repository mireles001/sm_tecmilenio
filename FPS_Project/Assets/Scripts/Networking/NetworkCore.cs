using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkCore : MonoBehaviour
{
  protected const int PORT = 26501;
  protected const int MAX_USERS = 8;
  protected const int BYTE_SIZE = 1024;
  protected const int CLIENT_UPDATE_TIME_MS = 500;
  protected const int SERVER_UPDATE_TIME_MS = 500;
  protected byte _reliableChannel;
  protected byte _unreliableChannel;
  protected byte _stateUpdateChannel;
  protected int _connectionId;
  protected bool _isStarted;
  protected bool _isServer;
  protected byte _error;
  protected int _hostId;
  protected string _serverIp;
  protected string _username;
  protected GameMaster _master;

  private void Update()
  {
    UpdateMessagePump();
  }

  public virtual void Init(GameMaster master)
  {
    _master = master;
    _serverIp = master.ServerIp;
    _isServer = master.IsServer;
    _username = master.Username;
    NetworkTransport.Init();
    _isStarted = true;
  }

  public void Shutdown()
  {
    _isStarted = false;

    NetworkTransport.Shutdown();

    Destroy(gameObject);
  }

  public virtual void UpdateMessagePump()
  {
    if (!_isStarted)
      return;
  }

  #region Send
  // Client Code
  public void SendServer(NetMsg msg, byte chan)
  {
    // This is where we hold our data
    byte[] buffer = new byte[BYTE_SIZE];

    // This is wehre you would crush your data into a byte[]
    BinaryFormatter formatter = new BinaryFormatter();
    MemoryStream ms = new MemoryStream(buffer);
    formatter.Serialize(ms, msg);
    NetworkTransport.Send(_hostId, _connectionId, chan, buffer, BYTE_SIZE, out _error);
  }

  // Server Code
  public void SendClient(int cnnId, NetMsg msg, byte chan)
  {
    // This is where we hold our data
    byte[] buffer = new byte[BYTE_SIZE];

    // This is wehre you would crush your data into a byte[]
    BinaryFormatter formatter = new BinaryFormatter();
    MemoryStream ms = new MemoryStream(buffer);
    formatter.Serialize(ms, msg);

    NetworkTransport.Send(_hostId, cnnId, chan, buffer, BYTE_SIZE, out _error);
  }

  public void LameBroadCast(NetMsg msg, byte chan)
  {
    var pls = GameState.GetInstance().players;
    foreach (KeyValuePair<int, PlayerInstance> entry in pls)
    {
      SendClient(entry.Key, msg, chan);
    }
  }
  #endregion

  public bool IsServer
  {
    get
    {
      return _isServer;
    }
  }
}
