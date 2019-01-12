using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
  [SerializeField]
  private Button _btnServer;
  [SerializeField]
  private Button _btnClient;
  [SerializeField]
  private Button _btnDisconnect;
  [SerializeField]
  private Button _btnKillServer;
  [SerializeField]
  private Text _txtConsoleOutput;

  private bool _isServer;
  private GameObject _networkGO;

  private void Awake()
  {
    _btnDisconnect.gameObject.SetActive(false);
    _btnKillServer.gameObject.SetActive(false);
  }

  public void StartUp(bool isServer)
  {
    _networkGO = new GameObject("Network");
    if (isServer)
    {
      _networkGO.AddComponent<Server>().Init();
    }
    else
    {
      _networkGO.AddComponent<Client>().Init();
    }
    _isServer = isServer;

    ToggleButtons();

    Debug.Log("Is Server: " + isServer);
  }

  public void Disconnect()
  {
    Shutdown();
    Debug.Log("Disconnect from Server");
  }

  public void KillServer()
  {
    Shutdown();
    Debug.Log("Kill Server");
  }

  public void ConsoleMsg(string msg)
  {
    _txtConsoleOutput.text += msg + "\n";
    Debug.Log(msg);
  }

  private void Shutdown()
  {
    ToggleButtons(true);
    _networkGO.SendMessage("Shutdown");
  }

  private void ToggleButtons(bool show = false)
  {
    _btnServer.gameObject.SetActive(show);
    _btnClient.gameObject.SetActive(show);

    if (_isServer)
    {
      _btnKillServer.gameObject.SetActive(!show);
    }
    else
    {
      _btnDisconnect.gameObject.SetActive(!show);
    }
  }
}
