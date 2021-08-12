using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Direction { get; protected set; } = 0;

    [SerializeField]
    private float _speed = 2;

    private void Update()
    {
        PlayerInput();

        if (Direction != 0) Move(Time.deltaTime);
    }

    // Keyboard input manager
    protected virtual void PlayerInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Direction = Input.GetAxis("Horizontal");
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(Input.touchCount - 1);

            float _tapDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
            Vector3 _tapPosition = new Vector3(touch.position.x, touch.position.y, _tapDistance);
            _tapPosition = Camera.main.ScreenToWorldPoint(_tapPosition);
            _tapDistance = _tapPosition.x - transform.position.x;

            if (Mathf.Abs(_tapDistance) > 2)
            {
                if (transform.position.x > _tapPosition.x) Direction = -1;
                else Direction = 1;
            }
        }
        else Direction = 0;
    }

    private void Move(float time)
    {
        Vector3 newPosition = transform.position;
        newPosition.x += Direction * _speed * time;

        transform.position = newPosition;
    }
}
