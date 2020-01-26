using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  private Animator _anim;
  private SpriteRenderer _sr;
  private PlayerMovement _movement;

  private void Start()
  {
    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();
    _movement = GetComponent<PlayerMovement>();
  }

  private void Update()
  {
    if (_movement.Direction > 0)
    {
      _sr.flipX = false;
    }
    else if (_movement.Direction < 0)
    {
      _sr.flipX = true;
    }

    _anim.SetBool("isGrounded", _movement.IsGrounded);
    _anim.SetFloat("movement", Mathf.Abs(_movement.Direction));
    _anim.SetFloat("verticalSpeed", _movement.Rb.velocity.y);
  }

  public void Jump()
  {
    _anim.Play("jump");
  }
}
