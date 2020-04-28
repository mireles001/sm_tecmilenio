using UnityEngine;

public class PickUpController : MonoBehaviour
{
  public ParticleSystem particles;

  public void Use(GameObject gameController)
  {
    if (particles) Instantiate(particles).transform.position = transform.position;

    gameController.SendMessage("PickedUpItem");
    Destroy(gameObject);
  }
}
