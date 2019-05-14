using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
  [SerializeField]
  private RectTransform _characterSelect;
  [SerializeField]
  private Text _charLabelFront;
  [SerializeField]
  private Text _charLabelBack;
  [SerializeField]
  private Button _characterSelectButton;
  [SerializeField]
  private Button _charAccept;
  [SerializeField]
  private Button _charDisconnect;
  [SerializeField]
  private Button _charEndMatch;
  [SerializeField]
  private InputField _username;

  private Button[] _buttons;
  [SerializeField]
  private AudioClip _click;
  private Animator _anim;
  private AudioSource _buttonsSFX;

  private int _temporalCharacter = -1;
  private bool _characterSelectActive = false;

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
    _buttonsSFX.clip = _click;
    _username.text = _master.Username;

    BuildCharacterSelect();

    if (!_master.IsServer)
    {
      ToogleCharacterSelect();
    }

    _charEndMatch.gameObject.SetActive(_master.IsServer);
    _charDisconnect.gameObject.SetActive(!_master.IsServer);

    _charAccept.onClick.AddListener(delegate () { ApplyChanges(); });
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
    Vector2 btnSize = new Vector2(256f, 256f);
    Vector2 btnPos = new Vector2(-550f, 50f);
    Image image;
    RectTransform rect;
    CharacterData data;
    for (int i = 0; i < _master.Roster.Length; i++)
    {
      Button newBtn = Instantiate(_characterSelectButton, _characterSelect);
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

      btnPos.x += 256f;
      if (i == 4)
      {
        btnPos.x = -676;
        btnPos.y += -215f;
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
      _charLabelBack.text = _charLabelFront.text = "";
      btn.OnDeselect(null);
    }
    else
    {
      _temporalCharacter = index;
      btn.OnSelect(null);
      _charLabelBack.text = _charLabelFront.text = _master.Roster[index].name;
    }
  }

  private void ApplyChanges()
  {
    if (_temporalCharacter > -1 && _username.text.Length > 0)
    {
      _master.SelectCharacter(_temporalCharacter, _username.text);

      ToogleCharacterSelect();
    }
  }
}
