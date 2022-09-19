using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Input
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private float playerMoveSpeed = 2f;
        [SerializeField] private float playerJumpStrength = 5f;
        [SerializeField] private float playerRespawnHeight = -5f;
        [SerializeField] private float playerRotationSpeed = 1f;
        
        private CharacterController _characterController;
        private Vector3 _respawnPosition;
        private Vector3 _playerVelocity;
        private Vector3 _targetDirection;
        private Vector2 _moveDirection;

        private bool _isGrounded;
        
        private Animator _playerAnimator;
        
        #region Unity Events
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _respawnPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (transform.position.y < playerRespawnHeight)
            {
                transform.position = _respawnPosition;
                _playerVelocity = Physics.gravity;
            }
            else
            {
                Transform camTransform = Camera.main.transform;
                Vector3 gravityDir = Physics.gravity.normalized;
                Vector3 rightDir = Vector3.Cross(-gravityDir, camTransform.forward).normalized;
                Vector3 forwardDir;
                float w = Vector3.Dot(-gravityDir, camTransform.forward);
                if (w > -1f && w < 1f)
                {
                    forwardDir = Vector3.Cross(rightDir, -gravityDir).normalized;
                }
                else
                {
                    forwardDir = w < 0 ? camTransform.up : -camTransform.up;
                    rightDir = camTransform.right;
                }

                Vector3 moveVector = rightDir * (_moveDirection.x * playerMoveSpeed) + forwardDir * (_moveDirection.y * playerMoveSpeed);
                
                if (_moveDirection.sqrMagnitude > 0f)
                    _targetDirection = moveVector;
                
                //_playerVelocity.x = _moveDirection.x * playerMoveSpeed;
                //_playerVelocity.z = _moveDirection.y * playerMoveSpeed;
                
                _characterController.Move((moveVector + _playerVelocity) * Time.fixedDeltaTime);

                if (_characterController.isGrounded)
                    _playerVelocity = Vector3.zero;

                _playerVelocity += Physics.gravity * Time.fixedDeltaTime;
                
                if (_targetDirection.sqrMagnitude > 0f)
                    _characterController.transform.rotation = Quaternion.Slerp(_characterController.transform.rotation, Quaternion.LookRotation(_targetDirection), playerRotationSpeed * Time.fixedDeltaTime);

                _playerAnimator.SetBool(AnimatorParameters.IsGrounded, _characterController.isGrounded);
                _playerAnimator.SetBool(AnimatorParameters.IsFalling, Vector3.Dot(_playerVelocity, Physics.gravity.normalized) > 0.5f);
                _playerAnimator.SetFloat(AnimatorParameters.Velocity, _playerVelocity.magnitude * Time.fixedDeltaTime);
            }
        }

        #endregion Unity Events
        
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
            
            _playerVelocity = -Physics.gravity.normalized * playerJumpStrength;
        }
    }
}
