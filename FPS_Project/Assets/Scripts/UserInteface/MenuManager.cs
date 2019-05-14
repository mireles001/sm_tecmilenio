using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _gameMaster;

  [SerializeField]
  private Image _fader;
  [SerializeField]
  private GameObject _title;
  [SerializeField]
  private GameObject _create;
  [SerializeField]
  private GameObject _join;
  [SerializeField]
  private AudioClip _click;

  [SerializeField]
  private Button _exit;
  [SerializeField]
  private Button _goToCreate;
  [SerializeField]
  private Button _goToJoin;
  [SerializeField]
  private Button _goBackJoin;
  [SerializeField]
  private Button _goBackCreate;
  [SerializeField]
  private Button _createServer;
  [SerializeField]
  private Button _joinServer;
  [SerializeField]
  private InputField _serverIp;
  [SerializeField]
  private InputField _connectIp;
  [SerializeField]
  private InputField _setNameServer;
  [SerializeField]
  private InputField _setNameClient;

  private Animator _anim;
  private AudioSource _bg;
  private AudioSource _buttonsSFX;
  private bool _killApp = false;
  private bool _goToLimbo = false;

  private string _username = "player";
  private string _myIp;
  private string _ip;
  private bool _isServer = false;

  private void Start()
  {
    _fader.gameObject.SetActive(true);
    _title.SetActive(false);
    _create.SetActive(false);
    _join.SetActive(false);

    _exit.onClick.AddListener(delegate () { BtnHandler("exit"); });
    _goToCreate.onClick.AddListener(delegate () { BtnHandler("goToCreate"); });
    _goToJoin.onClick.AddListener(delegate () { BtnHandler("goToJoin"); });
    _goBackJoin.onClick.AddListener(delegate () { BtnHandler(); });
    _goBackCreate.onClick.AddListener(delegate () { BtnHandler(); });

    _createServer.onClick.AddListener(delegate () { BtnHandler("create"); });
    _joinServer.onClick.AddListener(delegate () { BtnHandler("join"); });

    _anim = GetComponent<Animator>();
    _bg = GetComponent<AudioSource>();
    _buttonsSFX = gameObject.AddComponent<AudioSource>();
    _buttonsSFX.playOnAwake = false;
    _buttonsSFX.clip = _click;

    _anim.SetBool("at_create", false);
    _anim.SetBool("at_join", false);

    _serverIp.readOnly = true;
    _myIp = _serverIp.text = new IP().GetIp();

    CheckForGameMaster();
  }

  private void FadeInCb()
  {
    if (_killApp)
    {
      Debug.Log("Kill App");
      Application.Quit();
    }
    else if (_goToLimbo)
    {
      GoToLimbo();
    }
  }

  private void BtnHandler(string btnPressed = "goBack")
  {
    _buttonsSFX.Play();
    switch (btnPressed)
    {
      case "exit":
        _killApp = true;
        _anim.Play("fadeIn");
        break;
      case "goToCreate":
        _anim.SetBool("at_create", true);
        _anim.SetBool("at_join", false);
        break;
      case "goToJoin":
        _anim.SetBool("at_join", true);
        _anim.SetBool("at_create", false);
        break;
      case "create":
        _isServer = true;
        _goToLimbo = true;
        SetName(_setNameServer.text);
        _anim.Play("fadeIn");
        break;
      case "join":
        if (_connectIp.text.Length > 0)
        {
          _isServer = false;
          _goToLimbo = true;
          _ip = _connectIp.text;
          SetName(_setNameClient.text);
          _anim.Play("fadeIn");
        }
        break;
      default:
        _anim.SetBool("at_join", false);
        _anim.SetBool("at_create", false);
        break;
    }
  }

  private void SetName(string name)
  {
    if (name.Length > 0)
    {
      _username = name;
    }
  }

  public void GoToLimbo()
  {
    GameObject master = Instantiate(_gameMaster);
    master.name = "GameMaster";
    DontDestroyOnLoad(master);

    master.GetComponent<GameMaster>().StartUp(_isServer, _myIp, _ip, _username);

    SceneManager.LoadScene("limbo");
  }

  private void CheckForGameMaster()
  {
    GameObject master = GameObject.FindGameObjectWithTag("GameMaster");

    if (master)
    {
      Destroy(master);
    }
  }
}
