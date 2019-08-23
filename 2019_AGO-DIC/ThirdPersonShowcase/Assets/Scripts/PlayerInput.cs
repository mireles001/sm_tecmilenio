using UnityEngine;

public class PlayerInput : MonoBehaviour
{
  [SerializeField, Range(0f, 1f)]
  private float _minInput = 0.1f;
  public Vector3 MoveDirection { get; private set; } = Vector3.zero;
  public Vector3 LookDirection { get; private set; } = Vector3.zero;
  public bool ForceWalk { get; private set; } = false;

  private void Update()
  {
    MoveDirection = GetInput("Horizontal", "Vertical");
    LookDirection = GetInput("Mouse X", "Mouse Y");

    if (Input.GetButton("Cancel"))
    {
      ForceWalk = true;
    }
    else
    {
      ForceWalk = false;
    }
  }

  private Vector3 GetInput(string horizontal, string vertical)
  {
    float x = Mathf.Abs(Input.GetAxis(horizontal)) >= _minInput ? Input.GetAxis(horizontal) : 0f,
      y = Mathf.Abs(Input.GetAxis(vertical)) >= _minInput ? Input.GetAxis(vertical) : 0f;

    Vector3 direction = x * Vector3.right + y * Vector3.forward;

    if (direction != Vector3.zero && direction.magnitude > 1)
    {
      direction = direction.normalized;
    }

    return direction;
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(10, 10, 300, 20), "Move: " + MoveDirection);
    GUI.Label(new Rect(10, 35, 300, 20), "Look: " + LookDirection);
  }
}
