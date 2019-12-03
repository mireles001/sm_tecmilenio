using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public bool IsPaused { get; private set; } = true;
  public int SelectionCursor { get; set; }

  public CameraMovement cameraMovement;
  public Spinner cameraSpinner;
  public Button[] buttons;
  public Button closeButton;
  public GameObject[] characterPrefabs;
  public Animator menuAnimator;
  public Image fader;

  private CharacterMovement _characterMovement;
  private int _selectedCharacter = 0;
  private GameObject _character;
  private Color _faderColor;

  private AudioSource _as;

  private bool _fadingIn = true;
  private bool _fadingOut = false;
  [SerializeField]
  private float _fadeOutTimer = 1f;
  private float _currentTime = 0f;

  private void Awake()
  {
    fader.gameObject.SetActive(true);
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Start()
  {
    closeButton.Select();

    for (int i = 0; i < buttons.Length; i++)
    {
      int index = i;
      buttons[i].onClick.AddListener(delegate { SelectCharacter(index); });
      buttons[i].GetComponent<ButtonScript>().StartUp(i, this);
    }

    menuAnimator.SetBool("isShown", IsPaused);

    _as = GetComponent<AudioSource>();
    _faderColor = fader.color;
  }

  private void Update()
  {
    if (_fadingOut)
    {
      _currentTime += Time.deltaTime;

      float delta = _currentTime / _fadeOutTimer;
      _as.volume = Mathf.Lerp(1, 0, delta); ;
      _faderColor.a = Mathf.Lerp(0, 1, delta);
      fader.color = _faderColor;
      if (_currentTime  >= _fadeOutTimer)
      {
        Application.Quit();
      }
    }
    else
    {
      if (_fadingIn)
      {
        _currentTime += Time.deltaTime;
        _faderColor.a = Mathf.Lerp(1, 0, _currentTime / _fadeOutTimer);
        fader.color = _faderColor;

        if (_currentTime >= _fadeOutTimer)
        {
          fader.gameObject.SetActive(false);
          _fadingIn = false;
        }
      }
      else if (Input.GetButtonDown("Jump") && IsPaused)
      {
        SelectCharacter(SelectionCursor);
      }
      else if (Input.GetButtonDown("Submit") && !IsPaused)
      {
        ToggleMenu(true);
      }
      else if (Input.GetButtonDown("Cancel"))
      {
        ToggleMenu(!IsPaused);
      }
    }
  }

  public void SelectCharacter(int index)
  {
    SelectionCursor = index;

    if (_character == null || SelectionCursor != _selectedCharacter)
    {
      _selectedCharacter = index;

      Vector3 pos = Vector3.zero;
      Quaternion rot = Quaternion.identity;
      if (_character != null)
      {
        pos = _character.transform.position;
        rot = _character.transform.rotation;
        Destroy(_character);
      }

      _character = Instantiate(characterPrefabs[_selectedCharacter]);
      _character.transform.SetPositionAndRotation(pos, rot);

      _characterMovement = _character.GetComponent<CharacterMovement>();

      cameraMovement.Target = _character.transform;
      Vector3 lootAt = _character.transform.position;
      lootAt += Vector3.up * _character.GetComponent<CapsuleCollider>().height / 2f;
      cameraMovement.SetLookTarget(lootAt);
    }

    ToggleMenu(false);
  }

  public void ToggleMenu(bool value)
  {
    if (IsPaused == value || !value && !_character)
    {
      return;
    }

    IsPaused = value;

    if (IsPaused)
    {
      closeButton.Select();
      SelectionCursor = _selectedCharacter;
    }
    else
    {
      cameraMovement.Angle = cameraMovement.transform.eulerAngles.y;
    }

    cameraSpinner.enabled = IsPaused;
    cameraMovement.enabled = !IsPaused;
    _characterMovement.enabled = !IsPaused;
    menuAnimator.SetBool("isShown", IsPaused);
  }

  public void Restart()
  {
    SceneManager.LoadScene(0);
  }

  public void QuitApplication()
  {
    _currentTime = 0f;
    fader.gameObject.SetActive(true);
    _fadingOut = true;
  }
}
