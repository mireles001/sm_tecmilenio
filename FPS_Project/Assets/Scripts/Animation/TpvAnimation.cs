using UnityEngine;

public class TpvAnimation : MonoBehaviour
{
  private Animator _anim;
  private bool _isGrounded = false;

  private void Start()
  {
    _anim = GetComponent<Animator>();
  }

  public void SetIsGrounded(bool value)
  {
    _isGrounded = value;
    _anim.SetBool("is_grounded", _isGrounded);
  }

  public void SetIsRunning(bool value)
  {
    _anim.SetBool("is_running", value);
  }

  public void SetMovement(float value = 0f)
  {
    _anim.SetFloat("movement", value);
  }

  public void Jump()
  {
    SetIsGrounded(false);
    _anim.Play("jump_impulse");
  }

  public void Death()
  {
    if (_isGrounded)
    {
      _anim.Play("death");
    }
  }

  public void Damage()
  {
    if (_isGrounded)
    {
      _anim.Play("damage");
    }
  }

  public void WeaponChange()
  {
    _anim.Play("weapon_change");
  }

  public void WeaponUse()
  {
    _anim.Play("weapon_use");
  }
}
