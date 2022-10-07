using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
            if (!TeamManager.ActivePlayer || !context.started) return;

            //print("Pressed: [Jump]");
            
            TeamManager.ActivePlayer.InputController.Jump();
        }

        public void UsePrimary(InputAction.CallbackContext context)
        {
            if (!TeamManager.ActivePlayer || !context.started) return;

            if (TeamManager.ActivePlayer.HasWeapon)
                TeamManager.ActivePlayer.Weapon.UsePrimary();
        }

        public void UseSecondary(InputAction.CallbackContext context)
        {
            if (!TeamManager.ActivePlayer || !context.started) return;

            if (TeamManager.ActivePlayer.HasWeapon)
                TeamManager.ActivePlayer.Weapon.UseSecondary();
        }

        public void Inventory(InputAction.CallbackContext context)
        {
            if (!TeamManager.ActivePlayer || !context.started) return;

            TeamManager.ActivePlayer.Weapon = TeamManager.ActiveTeam.GetWeapon(GetPressedNumber() - 1);
            Debug.Log(TeamManager.ActivePlayer.Weapon);
        }
        
        public int GetPressedNumber() {
            for (int number = 0; number <= 9; number++)
            {
                if (UnityEngine.Input.GetKeyDown(number.ToString()))
                    return number;
            }
     
            return -1;
        }

    }
}