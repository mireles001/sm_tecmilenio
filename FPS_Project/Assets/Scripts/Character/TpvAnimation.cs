using UnityEngine;

public class TpvAnimation : MonoBehaviour
{
  public string changeWeaponAnimation = "";
  protected bool _isGrounded = false;
  protected Animator _animator;
  protected CharacterWeapons _weapons;
  [SerializeField]
  protected GameObject _sfxPrefab;
  protected CharacterSFX _sfx;
  protected int _weaponIndex;

  public virtual void Awake()
  {
    _animator = GetComponent<Animator>();
    _weapons = GetComponent<CharacterWeapons>();

    _sfx = Instantiate(_sfxPrefab, transform).GetComponent<CharacterSFX>();

    if (changeWeaponAnimation.Length > 0)
    {
      AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
      for (int i = 0; i < clips.Length; i++)
      {
        if (clips[i].name == changeWeaponAnimation)
        {
          _weapons.WeaponSwapTimer = clips[i].length / 2f;
          break;
        }
      }
    }
  }

  public void Jump()
  {
    SetIsGrounded(false);
    _animator.Play("jump_impulse");
    _sfx.Jump();
  }

  public void SetIsGrounded(bool value)
  {
    if (!_isGrounded && value)
    {
      _sfx.Land();
    }

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
    _animator.Play("weapon_change", -1, 0f);
    _weapons.WeaponChange(index);
    _weaponIndex = index;
    _sfx.WeaponSwap();
  }

  public void WeaponUse()
  {
    _animator.Play("weapon_use", -1, 0f);
    switch (_weaponIndex)
    {
      case 2:
        _sfx.FireGrenade();
        break;
      case 3:
        _sfx.FireRocket();
        break;
      default:
        _sfx.FireGun();
        break;
    }
  }

  public void Damage()
  {
    if (_isGrounded)
    {
      _animator.Play("damage", -1, 0f);
    }
    _sfx.Damage();
  }

  public void Death()
  {
    if (_isGrounded)
    {
      _animator.Play("death");
    }
    _sfx.Death();
  }

  public CharacterSFX Sfx
  {
    get
    {
      return _sfx;
    }
  }
}
