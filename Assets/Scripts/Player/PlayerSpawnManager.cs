using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [DisallowMultipleComponent]
    public class PlayerSpawnManager : MonoBehaviour
    {
        private static PlayerSpawnManager _instance;
        [SerializeField] private GameObject _playerPrefab;

        [SerializeField] private Material[] _bodyMaterials;
    
        private void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(gameObject);
        }

        public static PlayerSpawnManager GetInstance() => _instance;

        public Player SpawnPlayer(Vector3 position = new())
        {
            GameObject newPlayer = Instantiate(_playerPrefab, position, Quaternion.identity);
        
            MeshRenderer bodyRenderer = newPlayer.GetComponentInChildren<MeshRenderer>();
            bodyRenderer.material = _bodyMaterials[Random.Range(0, _bodyMaterials.Length)];
        
            return newPlayer.GetComponent<Player>();
        }
    }
}
