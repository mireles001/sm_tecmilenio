using UnityEngine;

public class PlayerMovementMouse : PlayerMovement
{
  protected bool _validTapPosition = false;
  protected float _tapDistance;
  [SerializeField]
  protected float _minTapDistance = 1f;
  [SerializeField]
  protected float _doubleTapSpeed = 0.25f;
  protected float _doubleTapTimer = 0f;
  protected Vector3 _tapPosition;

  protected float _inputValue = 0f;
  protected float _inputDragTimer;
  [SerializeField, Range(0f, 1f)]
  protected float _inputDrag = 0.5f;

  protected override void PlayerInput()
  {
    // Move
    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
    {
      _inputDragTimer = 0;
    }

    if (Input.GetMouseButton(0))
    {
      _tapDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
      _tapPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _tapDistance);
      _tapPosition = Camera.main.ScreenToWorldPoint(_tapPosition);
      _tapDistance = _tapPosition.x - transform.position.x;

      if (Mathf.Abs(_tapDistance) >= _minTapDistance)
      {
        if (!_validTapPosition)
        {
          _inputDragTimer = 0f;
        }

        if (transform.position.x > _tapPosition.x)
        {
          InputModifier(-1, Time.deltaTime);
        }
        else
        {
          InputModifier(1, Time.deltaTime);
        }
      }
      else
      {
        if (_validTapPosition)
        {
          _inputDragTimer = 0f;
        }

        InputModifier(0, Time.deltaTime);
      }
    }
    else
    {
      if (_inputValue != 0)
      {
        InputModifier(0, Time.deltaTime);
      }
    }

    Direction = _inputValue;

    // Jump
    if (Input.GetMouseButtonDown(0))
    {
      if (_doubleTapTimer == 0)
      {
        _doubleTapTimer += Time.deltaTime;
      }
      else if (_doubleTapTimer <= _doubleTapSpeed)
      {
        _doubleTapTimer = 0f;
        JumpHandler();
      }
    }

    if (_doubleTapTimer > 0)
    {
      _doubleTapTimer += Time.deltaTime;

      if (_doubleTapTimer > _doubleTapSpeed)
      {
        _doubleTapTimer = 0f;
      }
    }
  }

  protected void InputModifier(float lerpTo, float time)
  {
    _validTapPosition = lerpTo == 0 ? false : true;
    _inputDragTimer += time;
    _inputValue = Mathf.Lerp(_inputValue, lerpTo, _inputDragTimer / _inputDrag);
  }
}
