using UnityEngine;
using UnityEngine.Networking;

public class NetworkCore : MonoBehaviour
{
  protected const int PORT = 26501;
  protected const int WEB_PORT = 26502;
  protected const int MAX_USERS = 16;
  protected const int BYTE_SIZE = 1024;

  protected byte _reliableChannel;

  protected int _hostId;
  protected byte _error;

  protected bool _isStarted;

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

  public virtual void Init()
  {
    _isStarted = true;
    NetworkTransport.Init();
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
}
