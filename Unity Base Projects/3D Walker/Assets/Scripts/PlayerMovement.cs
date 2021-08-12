using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour, ICanJump
    {
        public Vector3 InputDirection { get { return _inputDirection; } }
        public bool IsGrounded { get; private set; } = false;
        public float VerticalVelocity { get { return _rigidbody.velocity.y; } }
        public Transform Ground { get; private set; }

        [SerializeField]
        private float _moveSpeed = 5;
        [SerializeField]
        private float _jumpForce = 10;
        [SerializeField]
        private float _turnSpeed = 0.1f;
        [SerializeField]
        private float _groundRotationSpeed = 5;
        [SerializeField]
        private LayerMask _groundMask = ~0;

        private float _turnVelocity;
        private Vector3 _inputDirection = Vector3.zero;
        private Vector3 _movementDirection = Vector3.zero;
        private Vector3 _groundCheckSize = new Vector3(0.4f, 0.14f, 0.4f);
        private RaycastHit _hit;
        private CapsuleCollider _collider;
        private Rigidbody _rigidbody;
        private PlayerInput _playerInput;
        private PlayerCamera _playerCamera;
        private Dictionary<string, ICanJump> _jumpComponents;

        private void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>().RegisterJump(this);
            _playerCamera = GetComponent<PlayerCamera>();

            Ground = new GameObject("Ground").transform;
            Ground.SetPositionAndRotation(transform.position, transform.rotation);
            Ground.SetParent(transform);
        }

        private void Update()
        {
            if (!_collider || !_rigidbody || !_playerCamera.Camera) return;

            _inputDirection = Vector3.zero;

            if (_playerInput != null)
            {
                _inputDirection.x = _playerInput.xAxis;
                _inputDirection.z = _playerInput.yAxis;
            }

            if (_inputDirection.magnitude > 1) _inputDirection = _inputDirection.normalized;

            RotatePlayer(_inputDirection);
        }

        private void FixedUpdate()
        {
            GroundCheck();
            MovePlayer(_inputDirection, _movementDirection);
        }

        private void RotatePlayer(Vector3 direction)
        {
            if (direction.magnitude < 0.1f) return;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _playerCamera.Camera.eulerAngles.y;
            _movementDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private void GroundCheck()
        {
            bool groundCheck = Physics.BoxCast(_collider.bounds.center, _groundCheckSize / 2, -transform.up, out _hit, transform.rotation, _collider.bounds.extents.y, _groundMask);


            if (groundCheck)
            {
                if (!IsGrounded) Landing(VerticalVelocity);

                Quaternion qTo = Quaternion.FromToRotation(Ground.up, _hit.normal) * Ground.rotation;
                Ground.rotation = Quaternion.Slerp(Ground.rotation, qTo, _groundRotationSpeed * Time.fixedDeltaTime);

                Vector3 rot = Ground.localEulerAngles;
                rot.y = 0;
                Ground.localEulerAngles = rot;
            }
            else
            {
                Ground.rotation = Quaternion.Slerp(Ground.rotation, transform.rotation, _groundRotationSpeed * Time.fixedDeltaTime);
            }

            IsGrounded = groundCheck;
        }

        private void MovePlayer(Vector3 input, Vector3 direction)
        {
            if (input.magnitude < 0.1f) return;

            if (direction.magnitude > 1) direction = direction.normalized;

            _rigidbody.MovePosition(_rigidbody.position + input.magnitude * direction * _moveSpeed * Time.fixedDeltaTime);
        }

        public void Jump()
        {
            if (!IsGrounded) return;

            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            JumpCallback();
        }

        public void Landing(float velocity)
        {
            LandingCallback();
        }

        public string ID { get { return GetInstanceID().ToString(); } }

        public PlayerMovement RegisterJump(ICanJump obj)
        {
            if (_jumpComponents == null) _jumpComponents = new Dictionary<string, ICanJump>();

            _jumpComponents[obj.ID] = obj;

            return this;
        }

        private void JumpCallback()
        {
            foreach (KeyValuePair<string, ICanJump> cb in _jumpComponents)
            {
                if (_jumpComponents[cb.Key] == null) _jumpComponents.Remove(cb.Key);
                else cb.Value.Jump();
            }
        }

        private void LandingCallback()
        {
            foreach (KeyValuePair<string, ICanJump> cb in _jumpComponents)
            {
                if (_jumpComponents[cb.Key] == null) _jumpComponents.Remove(cb.Key);
                else cb.Value.Landing(VerticalVelocity);
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 150, 25), "Vertical velocity: " + VerticalVelocity.ToString("F2"));
            GUI.Label(new Rect(10, 30, 150, 25), "Input direction: " + _inputDirection.magnitude.ToString("F2"));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position, _groundCheckSize);
        }
#endif
    }
}
