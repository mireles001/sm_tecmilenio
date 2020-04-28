using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  public Animator anim;

  private Vector3 _scale;
  private Transform _character;
  private PlayerMovement _movement;

  private enum Axis { x, y, z }
  [SerializeField]
  private Axis _flipAxis = Axis.x;

  private void Start()
  {
    _character = anim.transform;
    _movement = GetComponent<PlayerMovement>();

    _scale = _character.localScale;
  }

  private void Update()
  {
    if (_movement.Direction > 0)
    {
      switch(_flipAxis)
      {
        case Axis.y:
          _scale.y = 1;
          break;
        case Axis.z:
          _scale.z = 1;
          break;
        default:
          _scale.x = 1;
          break;
      }
    }
    else if (_movement.Direction < 0)
    {
      switch (_flipAxis)
      {
        case Axis.y:
          _scale.y = -1;
          break;
        case Axis.z:
          _scale.z = -1;
          break;
        default:
          _scale.x = -1;
          break;
      }
    }
    _character.localScale = _scale;

    anim.SetBool("isGrounded", _movement.IsGrounded);
    anim.SetFloat("velocity", Mathf.Abs(_movement.Direction));
    anim.SetFloat("verticalVelocity", _movement.Rb.velocity.y);
  }

  public void Jump()
  {
    anim.Play("jump", -1, 0);
  }

  public void Death()
  {
    anim.Play("death", -1, 0);
  }
}
