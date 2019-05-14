using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
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

  private Animator _anim;
  private AudioSource _bg;
  private AudioSource _buttonsSFX;
  private bool _killApp = false;
  private bool _goToLimbo = false;

  private void Start()
  {
    _fader.gameObject.SetActive(true);
    _title.SetActive(true);
    _create.SetActive(false);
    _join.SetActive(false);

    _exit.onClick.AddListener(delegate () { BtnHandler("exit"); });

    _anim = GetComponent<Animator>();
    _bg = GetComponent<AudioSource>();
    _buttonsSFX = gameObject.AddComponent<AudioSource>();
    _buttonsSFX.playOnAwake = false;
    _buttonsSFX.clip = _click;
  }

  public void FadeOutCb()
  {
    _fader.gameObject.SetActive(false);
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
      Debug.Log("Go to Limbo");
    }
  }

  private void BtnHandler(string btnPressed)
  {
    _buttonsSFX.Play();
    switch (btnPressed)
    {
      case "exit":
        _killApp = true;
        _fader.gameObject.SetActive(true);
        _anim.Play("fadeIn");
        break;
    }
  }

  
}
