using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
  public RectTransform characterSelect;
  public Text charLabelFront;
  public Text charLabelBack;
  public Button characterSelectButton;
  public Button charAccept;
  public Button charDisconnect;
  public Button charEndMatch;
  public InputField username;

  public Text consoleOutput;
  public AudioClip click;

  private int _temporalCharacter = -1;
  private bool _characterSelectActive = false;
  private Button[] _buttons;
  private Animator _anim;
  private AudioSource _buttonsSFX;
  private GameMaster _master;

  private void Awake()
  {
    _master = GetComponent<GameMaster>();
  }

  private void Start()
  {
    _anim = GetComponent<Animator>();
    _buttonsSFX = gameObject.AddComponent<AudioSource>();
    _buttonsSFX.playOnAwake = false;
    _buttonsSFX.clip = click;
    username.text = _master.Username;
    consoleOutput.text = "";

    BuildCharacterSelect();

    if (!_master.IsServer)
    {
      ToogleCharacterSelect();
    }

    charEndMatch.gameObject.SetActive(_master.IsServer);
    charDisconnect.gameObject.SetActive(!_master.IsServer);

    charAccept.onClick.AddListener(delegate () { ApplyChanges(); });
    charEndMatch.onClick.AddListener(delegate () { _master.KillServer(); });
    charDisconnect.onClick.AddListener(delegate () { _master.Disconnect(); });
  }

  private void LateUpdate()
  {
    if (!_master.MatchEnded)
    {
      if (Input.GetKeyDown("c") || Input.GetKeyDown("escape"))
      {
        ToogleCharacterSelect();
      }
    }
  }

  private void ToogleCharacterSelect()
  {
    _characterSelectActive = !_characterSelectActive;
    _anim.SetBool("show_characters", _characterSelectActive);
    _buttonsSFX.Play();

    if (_characterSelectActive && _master.SelectedCharacter > -1)
    {
      _buttons[_master.SelectedCharacter].OnSelect(null);
    }
  }

  private void BuildCharacterSelect()
  {
    _buttons = new Button[_master.Roster.Length];
    Vector2 btnSize = new Vector2(128f, 128f);
    Vector2 btnPos = new Vector2(-270f, 27f);
    Image image;
    RectTransform rect;
    CharacterData data;
    for (int i = 0; i < _master.Roster.Length; i++)
    {
      Button newBtn = Instantiate(characterSelectButton, characterSelect);
      data = _master.Roster[i].GetComponent<CharacterData>();
      rect = newBtn.GetComponent<RectTransform>();
      image = newBtn.GetComponent<Image>();

      int currentIndex = i;
      _buttons[currentIndex] = newBtn;
      newBtn.onClick.AddListener(() => SelectCharacter(currentIndex, newBtn));

      newBtn.name = "btn_" + _master.Roster[i].name;
      rect.sizeDelta = btnSize;
      rect.localPosition = btnPos;
      image.sprite = data.portrait;
      image.type = Image.Type.Simple;

      btnPos.x += 128f;
      if (i == 4)
      {
        btnPos.x = -334f;
        btnPos.y += -108f;
      }
    }
  }

  private void SelectCharacter(int index, Button btn)
  {
    _buttonsSFX.Play();
    if (_master.SelectedCharacter > -1)
    {
      _buttons[_master.SelectedCharacter].OnDeselect(null);
    }

    if (_temporalCharacter == index)
    {
      _temporalCharacter = -1;
      charLabelBack.text = charLabelFront.text = "";
      btn.OnDeselect(null);
    }
    else
    {
      _temporalCharacter = index;
      btn.OnSelect(null);
      charLabelBack.text = charLabelFront.text = _master.Roster[index].name;
    }
  }

  private void ApplyChanges()
  {
    if (_temporalCharacter > -1 && username.text.Length > 0)
    {
      _master.SelectCharacter(_temporalCharacter, username.text);
      ToogleCharacterSelect();
    }
  }

  public Text ConsoleOutput
  {
    get
    {
      return consoleOutput;
    }
  }
}
