using UnityEngine;

public class FpvAnimation : TpvAnimation
{
  private bool _isRunning = false;
  private float _inputVerticalValue;
  private float _inputHorizontalValue;
  private float _absoluteVertical;
  private CharacterController _char;

  public void StartUp()
  {
    CharacterCore core = GetComponent<CharacterCore>();
    _char = core.PlayerCore.PlayerMovement.CharRb;
    core.PlayerCore.Sfx = _sfx;
  }

  private void Update()
  {
    if (_char)
    {
      if (_char.isGrounded)
      {
        if (!_isGrounded)
        {
          _animator.SetBool("is_grounded", true);
          _sfx.Land();
        }

        _isGrounded = true;
        _inputVerticalValue = Input.GetAxis("Vertical");
        _absoluteVertical = Mathf.Abs(_inputVerticalValue);
        _inputHorizontalValue = Mathf.Abs(Input.GetAxis("Horizontal"));

        if (_absoluteVertical > 0.05f || _inputHorizontalValue > 0.05f)
        {
          if (!_isRunning)
          {
            _animator.SetBool("is_running", true);
          }

          _isRunning = true;

          if (_inputHorizontalValue > _absoluteVertical)
          {
            _animator.SetFloat("movement", _inputHorizontalValue);
          }
          else
          {
            _animator.SetFloat("movement", _inputVerticalValue);
          }
        }
        else
        {
          if (_isRunning)
          {
            _animator.SetBool("is_running", false);
          }

          _isRunning = false;
        }
      }
      else
      {
        if (_isGrounded)
        {
          _animator.SetBool("is_grounded", false);
          _sfx.Jump();
        }

        _isGrounded = false;
        _animator.SetFloat("verticalSpeed", _char.velocity.y);
      }
    }
  }
}
