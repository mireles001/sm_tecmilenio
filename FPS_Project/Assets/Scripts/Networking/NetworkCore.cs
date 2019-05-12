using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NetworkCore : MonoBehaviour
{
  protected const int PORT = 26501;
  protected const int MAX_USERS = 16;
  protected const int BYTE_SIZE = 1024;
  protected const int CLIENT_UPDATE_TIME_MS = 1000;
  protected const int SERVER_UPDATE_TIME_MS = 1000;

  protected byte _reliableChannel;
  protected byte _unreliableChannel;
  protected byte _stateUpdateChannel;
  protected int _connectionId;

  protected bool _isStarted;
  protected bool _isServer;
  protected byte _error;
  protected int _hostId;
  protected string _serverIp;

  protected ManagerUI _ui;

  #region Monobehavior
  private void Start()
  {
    DontDestroyOnLoad(gameObject);
  }

  private void Update()
  {
    UpdateMessagePump();
  }
  #endregion

  public bool IsServer()
  {
    return _isServer;
  }

  public virtual void Init(ManagerUI ui)
  {
    _ui = ui;

    _serverIp = ui._inputIP.textComponent.text;
    if (_serverIp.Length == 0)
      _serverIp = "127.0.0.1";

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
    NetworkTransport.Send(_hostId, _connectionId, chan , buffer, BYTE_SIZE, out _error);
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
    //NetworkTransport.GetBroadcastConnectionInfo;
    //NetworkTransport.GetBroadcastConnectionMessage;
    //NetworkTransport.SendMulticast()
    
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

  // public void TESTFUCNTION()
  // {
  //   Debug.Log("TESTFUNCTION");
  //   var p = GameState.GetInstance().localPlayer;
  //   Net_PlayerPushUpdate up = new Net_PlayerPushUpdate();
  //   up.posX = p.position.x;
  //   up.posY = p.position.y;
  //   up.posZ = p.position.z;
  //   SendServer(up);
    
  // }
}
