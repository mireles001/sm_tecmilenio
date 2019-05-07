using UnityEngine;

public class TpvAnimation : MonoBehaviour
{
  protected bool _isGrounded = false;
  protected Animator _animator;
  protected CharacterWeapons _weapons;

  public virtual void Awake()
  {
    _animator = GetComponent<Animator>();
    _weapons = GetComponent<CharacterWeapons>();
  }

  public virtual void Jump()
  {
    SetIsGrounded(false);
    _animator.Play("jump_impulse");
  }

  public void SetIsGrounded(bool value)
  {
    _isGrounded = value;
    _animator.SetBool("is_grounded", _isGrounded);
  }

  public void SetIsRunning(bool value)
  {
    _animator.SetBool("is_running", value);
  }

  public void SetMovement(float value = 0f)
  {
    _animator.SetFloat("movement", value);
  }

  public void WeaponChange(int index)
  {
    _animator.Play("weapon_change");
    _weapons.WeaponChange(index);
  }

  public void WeaponUse()
  {
    _animator.Play("weapon_use");
  }

  public void Damage()
  {
    if (_isGrounded)
      _animator.Play("damage");
  }

  public void Death()
  {
    if (_isGrounded)
      _animator.Play("death");
  }
}
