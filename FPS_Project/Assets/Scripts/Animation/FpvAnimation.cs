using UnityEngine;

public class FpvAnimation : MonoBehaviour
{
  private bool _isRunning = false;
  private bool _isGrounded = false;
  private float _inputValue;
  private PlayerMovement _player;
  private WeaponsManager _weapons;
  private Animator _anim;
  private CharacterController _char;

  private void Awake()
  {
    _anim = GetComponent<Animator>();

    Transform player = transform.parent;
    _weapons = GetComponent<WeaponsManager>().StartUp(player.GetComponent<PlayerWeapons>());
    player.GetComponent<PlayerMovement>().SetFpv(this);
  }

  public FpvAnimation StartUp(PlayerMovement player)
  {
    _player = player;
    _char = _player.gameObject.GetComponent<CharacterController>();

    return this;
  }

  private void LateUpdate()
  {
    if (_char)
    {
      if (_char.isGrounded)
      {
        if (!_isGrounded)
          _anim.SetBool("is_grounded", true);

        _isGrounded = true;
        _inputValue = Input.GetAxis("Vertical");

        if (Mathf.Abs(_inputValue) > 0.05f)
        {
          if (!_isRunning)
            _anim.SetBool("is_running", true);

          _isRunning = true;
          _anim.SetFloat("movement", _inputValue);
        }
        else
        {
          if (_isRunning)
            _anim.SetBool("is_running", false);

          _isRunning = false;
        }
      }
      else
      {
        if (_isGrounded)
          _anim.SetBool("is_grounded", false);

        _isGrounded = false;
        _anim.SetFloat("verticalSpeed", _char.velocity.y);
      }
    }

    if (Input.GetKeyDown("v"))
    {
      Damage();
    }
  }

  public void WeaponUse()
  {
    _anim.Play("weapon_use");
  }

  public void WeaponChange(int index)
  {
    _anim.Play("weapon_change");
    _weapons.WeaponChange(index);
  }

  public void Jump()
  {
    _anim.SetBool("is_grounded", false);
  }

  public void Damage()
  {
    _anim.Play("damage");
  }
}
