using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float lookSensitivity;
        [SerializeField] private bool lookFlipY;
        
        [SerializeField] private float audioMaster;
        [SerializeField] private float audioMenu;
        [SerializeField] private float audioMusic;
        [SerializeField] private float audioGame;
    }
}