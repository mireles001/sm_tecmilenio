using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
  private Animator _anim;
  public bool IsGrounded { get; set; } = true;
  public float MoveSpeed { get; set; } = 0f;
  public float VerticalSpeed { get; set; } = 0f;

  private void Start()
  {
    _anim = GetComponent<Animator>();
  }

  private void Update()
  {
    if (!_anim)
    {
      return;
    }

    _anim.SetBool("isGrounded", IsGrounded);
    _anim.SetFloat("speed", MoveSpeed);
    _anim.SetFloat("verticalSpeed", VerticalSpeed);
  }

  public void Jump()
  {
    _anim.Play("jump");
  }
}
