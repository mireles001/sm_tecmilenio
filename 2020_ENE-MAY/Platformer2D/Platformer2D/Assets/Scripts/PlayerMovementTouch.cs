using UnityEngine;

public class PlayerMovementTouch : PlayerMovementMouse
{
  protected override void PlayerInput()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(Input.touchCount - 1);

      if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
      {
        _inputDragTimer = 0;

        // Jump
        if (touch.phase == TouchPhase.Began)
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
      }

      _tapDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
      _tapPosition = new Vector3(touch.position.x, touch.position.y, _tapDistance);
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

    if (_doubleTapTimer > 0)
    {
      _doubleTapTimer += Time.deltaTime;

      if (_doubleTapTimer > _doubleTapSpeed)
      {
        _doubleTapTimer = 0f;
      }
    }

    Direction = _inputValue;
  }
}
