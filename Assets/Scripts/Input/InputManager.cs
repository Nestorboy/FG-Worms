using Game;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (!TeamManager.ActivePlayer) return;
            
            InputMoveDirection = context.ReadValue<Vector2>();
            
            if (context.performed || context.canceled)
                TeamManager.ActivePlayer.InputController.Move(InputMoveDirection);
        }

        public void Look(InputAction.CallbackContext context)
        {
            InputLookDirection = context.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height) * 360f;
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (!TeamManager.ActivePlayer) return;
            
            if (!context.started) return;
            //print("Pressed: [Jump]");
            
            TeamManager.ActivePlayer.InputController.Jump();
        }
    }
}