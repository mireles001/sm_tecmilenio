using UnityEngine;
using Player;

public class CharacterAnimation : MonoBehaviour, ICanJump
{
    [SerializeField]
    private Animator _animator = null;
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;

    private int _animMovementId = Animator.StringToHash("movement");
    private int _animGroundedId = Animator.StringToHash("isGrounded");
    private int _animJumpId = Animator.StringToHash("Jump");

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>().RegisterJump(this);
    }
    private void Update()
    {
        if (!_playerInput || !_playerMovement || !_animator) return;

        _animator.SetBool(_animGroundedId, _playerMovement.IsGrounded);

        Vector3 inputDirection = new Vector3(_playerInput.xAxis, 0, _playerInput.yAxis);
        _animator.SetFloat(_animMovementId, Mathf.Clamp(inputDirection.magnitude, 0, 1));
    }

    public void Jump() { _animator.Play(_animJumpId, -1, 0); }

    public void Landing() { }

    public string ID { get { return GetInstanceID().ToString(); } }
}
