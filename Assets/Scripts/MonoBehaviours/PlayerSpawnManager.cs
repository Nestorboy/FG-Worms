using System;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class PlayerSpawnManager : MonoBehaviour
{
    private static PlayerSpawnManager _instance;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Material[] bodyMaterials;
    
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
        GameObject newPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
        
        MeshRenderer bodyRenderer = newPlayer.GetComponentInChildren<MeshRenderer>();
        bodyRenderer.material = bodyMaterials[Random.Range(0, bodyMaterials.Length)];
        
        return newPlayer.GetComponent<Player>();
    }
}
