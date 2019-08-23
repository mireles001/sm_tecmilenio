using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
  public GameObject player;
  private PlayerInput _input;
  private CharacterMovement _movement;
  private Animator _anim;

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

    float xSpeed = Mathf.Abs(_input.MoveDirection.x),
      zSpeed = Mathf.Abs(_input.MoveDirection.z);
    if (xSpeed > 0 || zSpeed > 0)
    {
      float speed = xSpeed > zSpeed ? xSpeed : zSpeed;
      _anim.SetFloat("horizontalSpeed", speed);
    }
    else
    {
      _anim.SetFloat("horizontalSpeed", 0f);
    }

    _anim.SetFloat("verticalSpeed", _movement.Velocity.y);
  }

  public void PlayJump()
  {
    _anim.Play("jump");
  }
}
