using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
  public GameObject treasurePrefab;
  public Transform treasureHolder;
  public float openChestDelay = 1f;
  public float treasureAnimationSpeed = 1f;

  private Transform treasure;
  private Vector3 treasureSize;
  private float treasureAnimationCurrentSpeed = 0f;

  private void Update()
  {
    if (treasure && treasureAnimationCurrentSpeed < treasureAnimationSpeed)
    {
      treasureAnimationCurrentSpeed += Time.deltaTime;

      treasure.localScale = Vector3.Lerp(Vector3.zero, treasureSize, treasureAnimationCurrentSpeed / treasureAnimationSpeed);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      other.gameObject.SendMessage("OpeningChest", SendMessageOptions.DontRequireReceiver);

      GetComponent<Animator>().SetBool("opened", true);

      Invoke("OpenChest", openChestDelay);
    }
  }

  private void OpenChest()
  {
    treasure = Instantiate(treasurePrefab, treasureHolder).transform;

    treasureSize = treasure.localScale * 3f;

    treasure.localScale = Vector3.zero;
  }
}
