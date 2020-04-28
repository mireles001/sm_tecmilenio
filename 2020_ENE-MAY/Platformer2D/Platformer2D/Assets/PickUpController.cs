using UnityEngine;

public class PickUpController : MonoBehaviour
{
  public GameObject particulas;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Instantiate(particulas).transform.position = transform.position;
      Destroy(gameObject);
    }
  }
}
