using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerManager;

namespace Input
{
    [DisallowMultipleComponent]
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        
        public Vector2 InputMoveDirection;
        public Vector2 InputLookDirection;
        
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
            
            InputMoveDirection = context.ReadValue<Vector2>();
            
            ActivePlayer.InputController.Move(InputMoveDirection);
        }

        public void Look(InputAction.CallbackContext context)
        {
            InputLookDirection = context.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height) * 360f;
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (!ActivePlayer) return;
            
            if (!context.started) return;
            //print("Pressed: [Jump]");
            
            ActivePlayer.InputController.Jump();
        }
    }
}