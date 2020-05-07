using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
  public Button startButton;
  public Button quitButton;
  public AudioClip buttonSound;
  private AudioSource _source;

  private void Start()
  {
    startButton.onClick.AddListener(StartHandler);
    quitButton.onClick.AddListener(ExitHandler);

    _source = GetComponent<AudioSource>();
  }

  private void PlaySound()
  {
    if (_source && buttonSound) _source.PlayOneShot(buttonSound);
  }

  private void StartHandler()
  {
    startButton.enabled = false;
    PlaySound();
    Invoke("StartPlaying", 0.5f);
  }

  private void ExitHandler()
  {
    quitButton.enabled = false;
    PlaySound();
    Invoke("QuitApp", 0.5f);
  }

  private void StartPlaying()
  {
    SceneManager.LoadScene(1);
  }

  private void QuitApp()
  {
    Application.Quit();
  }
}
