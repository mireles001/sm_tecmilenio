using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCore : MonoBehaviour
{
  public Button quit;
  public Image fader;

  [SerializeField]
  private float _faderDuration = 0.5f;
  private float _timer = 0f;
  private Color _faderColor;

  private enum FaderState
  {
    Off,
    FadeIn,
    FadeOut
  }

  private FaderState _faderState = FaderState.FadeOut;

  private void Awake()
  {
    fader.gameObject.SetActive(true);
    _faderColor = fader.color;
    _faderColor.a = 1f;
    fader.color = _faderColor;
  }

  private void Start()
  {
    quit.onClick.AddListener(QuitButtonHanlder);
  }

  private void QuitButtonHanlder()
  {
    _faderState = FaderState.FadeIn;
    Invoke("QuitApp", _faderDuration);
  }

  private void QuitApp()
  {
    SceneManager.LoadScene(0);
  }

  private void Update()
  {

    if (_faderState == FaderState.FadeOut)
    {
      FaderAnimation(1f, 0f);
    }
    else if (_faderState == FaderState.FadeIn)
    {
      FaderAnimation(0f, 1f);
    }
  }

  private void FaderAnimation(float start, float end)
  {
    _timer += Time.deltaTime;
    _faderColor.a = Mathf.Lerp(start, end, _timer / _faderDuration);

    if (_timer >= _faderDuration)
    {
      _faderColor.a = end;
      _timer = 0f;
      _faderState = FaderState.Off;
    }

    fader.color = _faderColor;
  }
}
