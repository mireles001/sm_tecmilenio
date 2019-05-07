using UnityEngine;

public class FpvAnimation : TpvAnimation
{
  private bool _isRunning = false;
  private float _inputValue;
  private CharacterController _char;

  public void StartUp()
  {
    _char = GetComponent<CharacterCore>().PlayerCore.PlayerMovement.CharRb;
  }

  private void LateUpdate()
  {
    if (_char)
    {
      if (_char.isGrounded)
      {
        if (!_isGrounded)
          _animator.SetBool("is_grounded", true);

        _isGrounded = true;
        _inputValue = Input.GetAxis("Vertical");

        if (Mathf.Abs(_inputValue) > 0.05f)
        {
          if (!_isRunning)
            _animator.SetBool("is_running", true);

          _isRunning = true;
          _animator.SetFloat("movement", _inputValue);
        }
        else
        {
          if (_isRunning)
            _animator.SetBool("is_running", false);

          _isRunning = false;
        }
      }
      else
      {
        if (_isGrounded)
          _animator.SetBool("is_grounded", false);

        _isGrounded = false;
        _animator.SetFloat("verticalSpeed", _char.velocity.y);
      }
    }
  }

  public override void Jump()
  {
    _animator.SetBool("is_grounded", false);
  }

}
