using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
  public Button _btnServer;
  public Button _btnClient;
  public Button _btnDisconnect;
  public Button _btnKillServer;
  public Button _btnCreatePlayer;
  public Text _txtConsoleOutput;
  public Text _txtIP;
  public InputField _inputIP;
  public InputField _inputUsername;

  private bool _isServer;
  [SerializeField]
  private Transform _spawnPoint;
  [SerializeField]
  private GameObject _playerPrefab;
  private GameObject _player;
  private GameObject _networkGO;

  private string _myIp;

  private void Start()
  {
    _btnDisconnect.gameObject.SetActive(false);
    _btnKillServer.gameObject.SetActive(false);
    _btnCreatePlayer.gameObject.SetActive(false);
    _txtConsoleOutput.text = "";
    _txtIP.text = new IP().GetIp();
  }

  public void StartUp(bool isServer)
  {
    _networkGO = new GameObject("Network");
    if (isServer)
    {
      _networkGO.AddComponent<Server>().Init(this);
    }
    else
    {
      _networkGO.AddComponent<Client>().Init(this);
    }
    _isServer = isServer;

    ToggleButtons();
  }

  public void Disconnect()
  {
    Shutdown();
    ConsoleMsg("Disconnect from Server");
  }

  public void KillServer()
  {
    Shutdown();
    ConsoleMsg("Kill Server");
  }

  private void Shutdown()
  {
    ToggleButtons(true);
    Destroy(_player);
    _networkGO.SendMessage("Shutdown");
  }

  private void ToggleButtons(bool show = false)
  {
    _btnServer.gameObject.SetActive(show);
    _btnClient.gameObject.SetActive(show);
    _inputIP.gameObject.SetActive(show);

    if (_isServer)
    {
      _btnKillServer.gameObject.SetActive(!show);
    }
    else
    {
      _btnDisconnect.gameObject.SetActive(!show);
    }

    _btnCreatePlayer.gameObject.SetActive(!show);

  }

  public void CreatePlayer()
  {
    if (!_player)
    {
      _player = Instantiate(_playerPrefab) as GameObject;
    }

    _player.transform.position = _spawnPoint.position;
  }

  public GameObject GetPlayer()
  {
    return _player;
  }

  public void ConsoleMsg(string msg)
  {
    _txtConsoleOutput.text += msg + "\n";
    Debug.Log(msg);
  }
}
