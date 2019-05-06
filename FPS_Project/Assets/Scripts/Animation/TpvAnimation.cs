using UnityEngine;

public class TpvAnimation : MonoBehaviour
{
  private Animator _anim;
  private bool _isGrounded = false;
  private WeaponsManager _weapons;

  private void Awake()
  {
    _anim = GetComponent<Animator>();
    _weapons = GetComponent<WeaponsManager>();
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

  public void WeaponChange(int index)
  {
    _anim.Play("weapon_change");
    _weapons.WeaponChange(index);
  }

  public void WeaponUse()
  {
    _anim.Play("weapon_use");
  }
}
