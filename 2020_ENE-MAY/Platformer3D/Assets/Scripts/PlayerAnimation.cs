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

  private bool _pendingLanding = false;
  private SoundManager _sound;

  private void Start()
  {
    _character = anim.transform;
    _movement = GetComponent<PlayerMovement>();
    _scale = _character.localScale;

    _sound = GetComponent<SoundManager>();
  }

  private void Update()
  {
    if (_movement.Direction > 0)
    {
      switch(_flipAxis)
      {
        case Axis.y:
          _scale.y = Mathf.Abs(_scale.y);
          break;
        case Axis.z:
          _scale.z = Mathf.Abs(_scale.z);
          break;
        default:
          _scale.x = Mathf.Abs(_scale.x);
          break;
      }
    }
    else if (_movement.Direction < 0)
    {
      switch (_flipAxis)
      {
        case Axis.y:
          _scale.y = -Mathf.Abs(_scale.y);
          break;
        case Axis.z:
          _scale.z = -Mathf.Abs(_scale.z);
          break;
        default:
          _scale.x = -Mathf.Abs(_scale.x);
          break;
      }
    }
    _character.localScale = _scale;

    anim.SetBool("isGrounded", _movement.IsGrounded);
    anim.SetFloat("velocity", Mathf.Abs(_movement.Direction));
    anim.SetFloat("verticalVelocity", _movement.Rb.velocity.y);

    if (_sound && _pendingLanding && _movement.IsGrounded && _movement.Rb.velocity.y < 0)
    {
      _pendingLanding = false;
      _sound.SfxLand();
    }
  }

  public void PlayJump()
  {
    _pendingLanding = true;
    if (_sound) _sound.SfxJump();
    anim.Play("jump", -1, 0);
  }

  public void PlayDeath()
  {
    anim.Play("death", -1, 0);
  }
}
