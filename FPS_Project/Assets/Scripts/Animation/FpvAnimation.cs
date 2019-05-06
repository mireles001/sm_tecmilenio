using UnityEngine;

public class FpvAnimation : MonoBehaviour
{
  private bool _isRunning;
  private bool _isGrounded;
  private float _inputValue;
  private PlayerMovement _player;
  private Animator _anim;
  private CharacterController _char;

  private void Start()
  {
    _isRunning = false;
    _isGrounded = false;
    _anim = GetComponent<Animator>();

    transform.parent.parent.GetComponent<PlayerMovement>().SetFpv(this);
  }

  public void StartUp(PlayerMovement player)
  {
    _player = player;
    _char = _player.gameObject.GetComponent<CharacterController>();
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
          _anim.SetFloat("hSpeed", _inputValue);
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
        _anim.SetFloat("vSpeed", _char.velocity.y);
      }
    }
  }

  public void WeaponUse()
  {
    _anim.SetTrigger("weapon_use");
  }

  public void WeaponChange()
  {
    _anim.SetTrigger("weapon_change");
  }

  public void Jump()
  {
    _anim.SetBool("is_grounded", false);
  }
}
