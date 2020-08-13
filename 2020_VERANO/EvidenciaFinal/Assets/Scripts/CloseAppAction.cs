using UnityEngine;

public class CloseAppAction : MonoBehaviour
{
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Debug.Log("Quit Application");
      Application.Quit();
    }
  }
}
