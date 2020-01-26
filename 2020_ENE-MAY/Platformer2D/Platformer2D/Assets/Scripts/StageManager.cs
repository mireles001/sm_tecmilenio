using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
  [SerializeField]
  private float _restartTimer = 2f;

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
}
