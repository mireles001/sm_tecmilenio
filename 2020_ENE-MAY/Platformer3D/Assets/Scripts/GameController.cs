using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  [SerializeField]
  private float _restartTimer = 2f;

  public int pickedUpItems = 0;

  public void GameOver()
  {
    Debug.Log("GameOver");
    Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;
    Invoke("Restart", _restartTimer);
  }

  private void Restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void Finish()
  {
    Debug.Log("You Win!");
  }

  public void PickedUpItem()
  {
    pickedUpItems++;
    Debug.Log("Items pickedup: " + pickedUpItems);
  }
}
