using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] public float DefaultLookSensitivity;
        [SerializeField] public bool DefaultLookFlipY;
        
        [SerializeField] public float DefaultAudioMaster;
        [SerializeField] public float DefaultAudioMenu;
        [SerializeField] public float DefaultAudioMusic;
        [SerializeField] public float DefaultAudioGame;

        public float LookSensitivity;
        public bool LookFlipY;
        
        public float AudioMaster;
        public float AudioMenu;
        public float AudioMusic;
        public float AudioGame;
        
        private bool _useDefaults;
        
        private void Awake()
        {
            LoadSettings();
        }

        public void SaveSettings()
        {
            if (_useDefaults)
            {
                _useDefaults = false;
                PlayerPrefs.SetInt(PrefKeys.UseDefaults, Convert.ToInt32(_useDefaults));
            }
            
            PlayerPrefs.SetFloat(PrefKeys.LookSensitivity, LookSensitivity);
            PlayerPrefs.SetInt(PrefKeys.LookFlipY, Convert.ToInt32(LookFlipY));

            PlayerPrefs.SetFloat(PrefKeys.AudioMaster, AudioMaster);
            PlayerPrefs.SetFloat(PrefKeys.AudioMenu, AudioMenu);
            PlayerPrefs.SetFloat(PrefKeys.AudioMusic, AudioMusic);
            PlayerPrefs.SetFloat(PrefKeys.AudioGame, AudioGame);
            
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            _useDefaults = Convert.ToBoolean(PlayerPrefs.GetInt(PrefKeys.UseDefaults, 1));

            LookSensitivity = PlayerPrefs.GetFloat(PrefKeys.LookSensitivity, DefaultLookSensitivity);
            LookFlipY = Convert.ToBoolean(PlayerPrefs.GetInt(PrefKeys.LookFlipY, Convert.ToInt32(DefaultLookFlipY)));

            AudioMaster = PlayerPrefs.GetFloat(PrefKeys.AudioMaster, DefaultAudioMaster);
            AudioMenu = PlayerPrefs.GetFloat(PrefKeys.AudioMenu, DefaultAudioMenu);
            AudioMusic = PlayerPrefs.GetFloat(PrefKeys.AudioMusic, DefaultAudioMusic);
            AudioGame = PlayerPrefs.GetFloat(PrefKeys.AudioGame, DefaultAudioGame);
        }

        public void ResetSettings()
        {
            if (_useDefaults)
                return;

            _useDefaults = true;
            
            LookSensitivity = DefaultLookSensitivity;
            LookFlipY = DefaultLookFlipY;
                
            AudioMaster = DefaultAudioMaster;
            AudioMenu = DefaultAudioMenu;
            AudioMusic = DefaultAudioMusic;
            AudioGame = DefaultAudioGame;

            PlayerPrefs.DeleteKey(PrefKeys.UseDefaults);

            PlayerPrefs.DeleteKey(PrefKeys.LookSensitivity);
            PlayerPrefs.DeleteKey(PrefKeys.LookFlipY);

            PlayerPrefs.DeleteKey(PrefKeys.AudioMaster);
            PlayerPrefs.DeleteKey(PrefKeys.AudioMenu);
            PlayerPrefs.DeleteKey(PrefKeys.AudioMusic);
            PlayerPrefs.DeleteKey(PrefKeys.AudioGame);

            PlayerPrefs.Save();
            
        }
    }
}