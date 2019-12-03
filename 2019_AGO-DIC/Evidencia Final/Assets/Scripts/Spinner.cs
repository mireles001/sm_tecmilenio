using UnityEngine;

public class Spinner : MonoBehaviour
{
  [SerializeField]
  private float _spinSpeed = 10f;

  private void Update()
  {
    if (_spinSpeed != 0f)
    {
      transform.Rotate(0f, _spinSpeed * Time.deltaTime, 0f);
    }
  }
}
