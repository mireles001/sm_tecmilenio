using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
  public GameObject player;
  private PlayerInput _input;
  private CharacterMovement _movement;
  private Animator _anim;

  private bool _queueJump = false;

  private void Awake()
  {
    _anim = GetComponent<Animator>();
  }

  private void Start()
  {
    _input = player.GetComponent<PlayerInput>();
    _movement = player.GetComponent<CharacterMovement>();
    _movement.SetCharacterAnimations(this);
  }

  private void Update()
  {
    _anim.SetBool("isGrounded", _movement.IsGrounded);
    _anim.SetFloat("verticalSpeed", _movement.Velocity.y);

    if (_movement.IsGrounded)
    {
      float xSpeed = Mathf.Abs(_input.MoveDirection.x),
      zSpeed = Mathf.Abs(_input.MoveDirection.z);
      if (xSpeed > 0 || zSpeed > 0)
      {
        float speed = xSpeed > zSpeed ? xSpeed : zSpeed;

        if (_input.ForceWalk && _movement.IsGrounded)
        {
          speed = Mathf.Clamp(speed, 0f, 0.5f);
        }
        _anim.SetFloat("horizontalSpeed", speed);
      }
      else
      {
        _anim.SetFloat("horizontalSpeed", 0f);
      }

      if (_queueJump)
      {
        _anim.SetTrigger("jump");
        _queueJump = false;
      }
    }
    else if (_movement.Velocity.y <= 0f)
    {
      _anim.ResetTrigger("jump");
    }
  }

  public void PlayJump()
  {
    _queueJump = true;
  }
}
