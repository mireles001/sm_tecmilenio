using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
  private string _username = "player";
  private string _myIp;
  private string _serverIp;
  private bool _isServer = false;

  [SerializeField]
  private GameObject _rosterPrefab;
  private GameObject[] _roster;
  private int _selectedChar = -1;

  private bool _matchExists = false;
  private bool _matchResults = false;

  private void Awake()
  {
    _roster = _rosterPrefab.GetComponent<CharactersReference>().characters;
  }

  public void StartUp(bool isServer, string myIp, string serverIp, string name)
  {
    _username = name;
    _myIp = myIp;
    _serverIp = serverIp;
    _isServer = isServer;

    Debug.Log("Is Server? " + _isServer);
  }

  public void SelectCharacter(int charIndex, string name)
  {
    _username = name;
    _selectedChar = charIndex;
    Debug.Log("Set values to create character later one");
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
}
