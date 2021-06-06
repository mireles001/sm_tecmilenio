using UnityEngine;
using Cinemachine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform Camera { private set; get; }

        [SerializeField]
        private CinemachineBrain _cmb = null;
        [SerializeField, Range(0, 100)]
        private int _lookAtHeight = 50;
        [SerializeField]
        private float _lookAtMaxDistance = 1;
        [SerializeField]
        private float _followerDamp = 0.5f;

        private float _lookAtY;
        private float _currentFollowerDamp;
        private Vector3 _currentVerticalAppend;
        private Vector3 _followerVelocity = Vector3.zero;
        private Transform _lookAt;
        private Transform _lookAtFollower;
        private CinemachineFreeLook _cmfl;
        private PlayerMovement _playerMovement;

        private void Start()
        {
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            float playerHeight = col ? col.height : 0;
            Vector3 position = transform.position;
            _lookAtY = (_lookAtHeight / 100f) * playerHeight;
            position.y += _lookAtY;

            Camera = _cmb.OutputCamera.transform;

            _cmfl = _cmb.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineFreeLook>();
            _playerMovement = GetComponent<PlayerMovement>();

            _lookAt = new GameObject("LookAt").transform;
            _lookAt.SetPositionAndRotation(position, transform.rotation);
            _lookAt.SetParent(transform);

            _lookAtFollower = new GameObject("LookAt Follower").transform;
            _lookAtFollower.SetPositionAndRotation(_lookAt.position, _lookAt.rotation);

            _cmfl.LookAt = _lookAtFollower;
        }

        private void FixedUpdate()
        {
            if (!_lookAt) return;

            _lookAtFollower.position = Vector3.SmoothDamp(_lookAtFollower.position, _lookAt.position, ref _followerVelocity, _currentFollowerDamp);
        }

        private void Update()
        {
            if (!_lookAt) return;

            float lerpSpeed = Time.deltaTime * 2.5f;

            float currentDamp = _playerMovement.IsGrounded ? _followerDamp : _followerDamp * 0.15f;
            _currentFollowerDamp = Mathf.Lerp(_currentFollowerDamp, currentDamp, lerpSpeed);

            Vector3 currentVerticalAppend = Vector3.zero;
            if (!_playerMovement.IsGrounded && _playerMovement.VerticalVelocity < -0.15f) currentVerticalAppend.y = -_lookAtY * 1.5f;

            _currentVerticalAppend = Vector3.Lerp(_currentVerticalAppend, currentVerticalAppend, lerpSpeed);
        }

        private void LateUpdate()
        {
            if (!_playerMovement || !_lookAt) return;

            _lookAt.rotation = _playerMovement.Ground.rotation;
            Vector3 newPosition;
            if (_playerMovement.InputDirection.magnitude > 0.01f)
            {
                newPosition = (transform.position + transform.up * _lookAtY) + (_lookAt.forward * _playerMovement.InputDirection.magnitude * _lookAtMaxDistance) + _currentVerticalAppend;
                _lookAt.position = newPosition;
            }
            else
            {
                newPosition = new Vector3(0, _lookAtY, 0) + _currentVerticalAppend;
                _lookAt.localPosition = newPosition;
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 50, 200, 25), "Camera follow damp: " + _currentFollowerDamp.ToString("F3"));
            GUI.Label(new Rect(10, 75, 200, 25), "Vertical append: " + _currentVerticalAppend.y.ToString("F2"));
        }

        private void OnDrawGizmos()
        {
            if (!_lookAt) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + Vector3.up * _lookAtY, _lookAtFollower.position);
            Gizmos.DrawWireSphere(_lookAtFollower.position, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_lookAt.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_lookAt.position, _lookAtFollower.position);
        }
#endif
    }
}
