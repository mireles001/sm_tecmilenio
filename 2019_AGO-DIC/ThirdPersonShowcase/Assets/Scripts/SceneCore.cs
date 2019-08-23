using UnityEngine;
using UnityEngine.UI;

public class SceneCore : MonoBehaviour
{
  public Button quit;

  private void Start()
  {
    quit.onClick.AddListener(delegate { Application.Quit(); });
  }
}
