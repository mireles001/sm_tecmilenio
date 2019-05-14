using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
  public string username;
  private string _myIp;
  private string _serverIp;
  private bool _isServer = true;

  public void StartUp(bool isServer, string myIp, string serverIp, string name)
  {
    username = name;
    _myIp = myIp;
    _serverIp = serverIp;
    _isServer = isServer;

    Debug.Log("Is Server? " + _isServer);
  }

  public string ServerIp
  {
    get
    {
      return _serverIp;
    }
  }

  public bool IsServer
  {
    get
    {
      return _isServer;
    }
  }
}
