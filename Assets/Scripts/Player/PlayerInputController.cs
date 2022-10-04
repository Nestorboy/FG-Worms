using UnityEngine;
using Visuals;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private float _playerMoveSpeed = 3f;
        [SerializeField] private float _playerJumpStrength = 5f;
        [SerializeField] private float _playerRespawnHeight = -5f;
        [SerializeField] private float _playerRotationSpeed = 10f;
        
        private CharacterController _characterController;
        private Vector3 _respawnPosition;
        private Vector3 _playerVelocity;
        private Vector3 _targetDirection;
        private Vector2 _moveDirection;

        private bool _isGrounded;
        
        private Animator _playerAnimator;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _respawnPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _playerVelocity += Physics.gravity * Time.fixedDeltaTime;
            
            Transform camTransform = Camera.main.transform;
            Vector3 gravityDir = Physics.gravity.normalized;
                
            float w = Vector3.Dot(-gravityDir, camTransform.forward);
            Vector3 rightDir = w > -1f && w < 1f ? Vector3.Cross(-gravityDir, camTransform.forward).normalized : camTransform.right;
            Vector3 forwardDir = Vector3.Cross(rightDir, -gravityDir).normalized;

            Vector3 moveVector = rightDir * (_moveDirection.x * _playerMoveSpeed) + forwardDir * (_moveDirection.y * _playerMoveSpeed);
                
            if (_moveDirection.sqrMagnitude > 0f)
                _targetDirection = moveVector;
                
            //_playerVelocity.x = _moveDirection.x * playerMoveSpeed;
            //_playerVelocity.z = _moveDirection.y * playerMoveSpeed;
                
            _characterController.Move((moveVector + _playerVelocity) * Time.fixedDeltaTime);

            if (transform.position.y < _playerRespawnHeight)
            {
                transform.position = _respawnPosition;
                _playerVelocity = Vector3.zero;
            }
            else if (_characterController.isGrounded)
            {
                Vector3 flatVelocity = Vector3.ProjectOnPlane(_playerVelocity, gravityDir);
                _playerVelocity = flatVelocity * 0.9f;
            }
                
            if (_targetDirection.sqrMagnitude > 0f)
                _characterController.transform.rotation = Quaternion.Slerp(_characterController.transform.rotation, Quaternion.LookRotation(_targetDirection), _playerRotationSpeed * Time.fixedDeltaTime);

            _playerAnimator.SetBool(AnimatorParameters.IsGrounded, _characterController.isGrounded);
            _playerAnimator.SetBool(AnimatorParameters.IsFalling, Vector3.Dot(_playerVelocity, Physics.gravity.normalized) > 0.5f);
            _playerAnimator.SetFloat(AnimatorParameters.Velocity, _playerVelocity.magnitude * Time.fixedDeltaTime);
        }

        public void ImpulseVelocity(Vector3 direction)
        {
            _playerVelocity += direction;
        }
        
        public void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude > 0f)
            {
                _moveDirection = direction / Mathf.Max(direction.magnitude, 1f);
                _targetDirection = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
            }
            else
            {
                _moveDirection = Vector2.zero;
            }
            
            _playerAnimator.SetBool(AnimatorParameters.IsWalking, direction.sqrMagnitude > 0f);
        }

        public void Jump()
        {
            if (!_characterController.isGrounded)
                return;
            
            _playerVelocity = -Physics.gravity.normalized * _playerJumpStrength;
        }
    }
}
