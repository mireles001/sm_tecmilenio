using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  public Button firstPersonDemo;
  public Button thirdPersonDemo;
  public Button quitApp;

    private void Start()
    {
    firstPersonDemo.onClick.AddListener( delegate { SceneManager.LoadScene(1); });
    thirdPersonDemo.onClick.AddListener( delegate { SceneManager.LoadScene(2); });
    quitApp.onClick.AddListener( delegate { Application.Quit(); });
    }
}
