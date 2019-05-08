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
        _inputVerticalValue = Input.GetAxis("Vertical");
        _absoluteVertical = Mathf.Abs(_inputVerticalValue);
        _inputHorizontalValue = Mathf.Abs(Input.GetAxis("Horizontal"));

        if (_absoluteVertical > 0.05f || _inputHorizontalValue > 0.05f)
        {
          if (!_isRunning)
            _animator.SetBool("is_running", true);

          _isRunning = true;

          if (_inputHorizontalValue > _absoluteVertical)
            _animator.SetFloat("movement", _inputHorizontalValue);
          else
            _animator.SetFloat("movement", _inputVerticalValue);

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
