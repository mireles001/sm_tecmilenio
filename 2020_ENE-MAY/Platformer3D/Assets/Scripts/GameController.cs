using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public GameObject win;
  public GameObject lose;
  public Text pickUpCounter;
  public CanvasGroup pausePanel;
  public Button resumeButton;
  public Button exitButton;
  [SerializeField]
  private float _restartTimer = 2f;
  private int _pickedUpItems = 0;
  private bool _isPaused = false;

  private SoundManager _sound;

  private void Start()
  {
    SetToggleValues();

    win.SetActive(false);
    lose.SetActive(false);

    resumeButton.onClick.AddListener(delegate { TogglePause(true); });
    exitButton.onClick.AddListener(ExitSceneHanlder);

    _sound = GameObject.FindGameObjectWithTag("Player").GetComponent<SoundManager>();
  }

  public void GameOver()
  {
    lose.SetActive(true);
    Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;
    Invoke("Restart", _restartTimer);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      TogglePause(_isPaused);
    }

    pickUpCounter.text = _pickedUpItems.ToString();
  }

  private void TogglePause(bool currentState)
  {
    _isPaused = !currentState;
    _sound.SfxButton();
    SetToggleValues();
  }

  private void SetToggleValues()
  {
    pausePanel.interactable = _isPaused;

    if (_isPaused)
    {
      pausePanel.alpha = 1;
      Time.timeScale = 0;
    }
    else
    {
      Time.timeScale = 1;
      pausePanel.alpha = 0;
    }
  }

  private void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void Finish(Vector3 finishPosition)
  {
    win.SetActive(true);
    Camera.main.gameObject.GetComponent<CameraMovement>().FinishGame(finishPosition);
  }

  public void PickedUpItem()
  {
    _pickedUpItems++;
  }

  private void ExitSceneHanlder()
  {
    _sound.SfxButton();
    pausePanel.enabled = false;
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    player.GetComponent<PlayerMovement>().LockPlayer();
    player.GetComponent<Rigidbody>().useGravity = false;
    Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;
    Time.timeScale = 1;
    Invoke("ExitScene", 0.5f);
  }

  public void ExitScene()
  {
    SceneManager.LoadScene(0);
  }
}
