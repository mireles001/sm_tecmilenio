using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
  public GameObject rosterPrefab;
  private string _username = "player";
  private string _myIp;
  private string _serverIp;
  private bool _isServer = false;
  private GameObject[] _roster;
  private int _selectedChar = -1;
  private bool _matchExists = false;
  private bool _matchResults = false;
  private GameUI _ui;
  private GameObject _networkGO;

  private void Awake()
  {
    gameObject.name = "GameMaster";
    DontDestroyOnLoad(gameObject);

    _roster = rosterPrefab.GetComponent<CharactersReference>().characters;
    _ui = GetComponent<GameUI>();
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log("OnSceneLoaded: " + scene.name + ", mode: " + mode);

    if (scene.name == "limbo" && !_networkGO)
    {
      CreateNetwork();
    }
  }

  public void StartUp(bool isServer, string myIp, string serverIp, string name)
  {
    _username = name;
    _myIp = myIp;
    _serverIp = serverIp;
    _isServer = isServer;
  }

  private void CreateNetwork()
  {
    _networkGO = new GameObject("Network");
    _networkGO.transform.parent = transform;
    if (_isServer)
      _networkGO.AddComponent<Server>().Init(this);
    else
      _networkGO.AddComponent<Client>().Init(this);
  }

  public void SelectCharacter(int charIndex, string name)
  {
    _username = name;
    _selectedChar = charIndex;
    Debug.Log("Set values to create character later one");
  }

  public void StartMatch()
  {
    Debug.Log("Start match");
  }

  public void EndMatch()
  {
    Debug.Log("End match");
  }

  public void Disconnect()
  {
    ConsoleMsg("Disconnect from Server");
    Shutdown();
  }

  public void KillServer()
  {
    ConsoleMsg("Kill Server");
    Shutdown();
  }

  private void Shutdown()
  {
    _networkGO.SendMessage("Shutdown");
    SceneManager.LoadScene("menu");
  }

  public void ConsoleMsg(string msg)
  {
    _ui.ConsoleOutput.text += msg + "\n";
    Debug.Log(msg);
  }

#region GetSets
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

  public GameObject[] Roster
  {
    get
    {
      return _roster;
    }
  }

  public string Username
  {
    get
    {
      return _username;
    }
  }

  public int SelectedCharacter
  {
    get
    {
      return _selectedChar;
    }
  }

  public bool MatchEnded
  {
    get
    {
      return _matchResults;
    }
  }

  public bool PlayingMatch
  {
    get
    {
      return _matchExists;
    }
  }

  public GameObject NetworkGO
  {
    get
    {
      return _networkGO;
    }
  }
  #endregion
}
