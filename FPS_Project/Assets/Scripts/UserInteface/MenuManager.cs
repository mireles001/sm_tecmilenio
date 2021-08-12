using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public GameObject gameMaster;

  public Image fader;
  public GameObject title;
  public GameObject create;
  public GameObject join;
  public AudioClip click;

  public Button exit;
  public Button goToCreate;
  public Button goToJoin;
  public Button goBackJoin;
  public Button goBackCreate;
  public Button createServer;
  public Button joinServer;
  public InputField serverIp;
  public InputField connectIp;
  public InputField setNameServer;
  public InputField setNameClient;

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
    fader.gameObject.SetActive(true);
    title.SetActive(false);
    create.SetActive(false);
    join.SetActive(false);

    exit.onClick.AddListener(delegate () { BtnHandler("exit"); });
    goToCreate.onClick.AddListener(delegate () { BtnHandler("goToCreate"); });
    goToJoin.onClick.AddListener(delegate () { BtnHandler("goToJoin"); });
    goBackJoin.onClick.AddListener(delegate () { BtnHandler(); });
    goBackCreate.onClick.AddListener(delegate () { BtnHandler(); });

    createServer.onClick.AddListener(delegate () { BtnHandler("create"); });
    joinServer.onClick.AddListener(delegate () { BtnHandler("join"); });

    setNameServer.text = setNameClient.text = _username;

    _anim = GetComponent<Animator>();
    _bg = GetComponent<AudioSource>();
    _buttonsSFX = gameObject.AddComponent<AudioSource>();
    _buttonsSFX.playOnAwake = false;
    _buttonsSFX.clip = click;

    _anim.SetBool("at_create", false);
    _anim.SetBool("at_join", false);

    serverIp.readOnly = true;
    _myIp = serverIp.text = new IP().GetIp();

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
        _ip = _myIp;
        SetName(setNameServer.text);
        _anim.Play("fadeIn");
        break;
      case "join":
        _isServer = false;
        _goToLimbo = true;
        _ip = connectIp.text;
        SetIp(connectIp.text);
        SetName(setNameClient.text);
        _anim.Play("fadeIn");
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
      _username = name;
  }

  private void SetIp(string ip)
  {
    if (ip.Length == 0)
      ip = "127.0.0.1";

    _ip = ip;
  }

  public void GoToLimbo()
  {
    Instantiate(gameMaster).GetComponent<GameMaster>().StartUp(_isServer, _myIp, _ip, _username);
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
