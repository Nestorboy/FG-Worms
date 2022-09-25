using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerManager;

namespace Input
{
    [DisallowMultipleComponent]
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        
        private Vector2 _inputMoveDirection;
        public Vector2 inputLookDirection;
        
        private void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(gameObject);
        }
        
        public static InputManager GetInstance() => _instance;
        
        public void Move(InputAction.CallbackContext context)
        {
            if (!ActivePlayer) return;
            
            _inputMoveDirection = context.ReadValue<Vector2>();
            
            ActivePlayer.playerController.Move(_inputMoveDirection);
        }

        public void Look(InputAction.CallbackContext context)
        {
            inputLookDirection = context.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height) * 360f;
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (!ActivePlayer) return;
            
            if (!context.started) return;
            //print("Pressed: [Jump]");
            
            ActivePlayer.playerController.Jump();
        }
    }
}