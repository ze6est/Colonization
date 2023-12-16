using System.Collections;
using Assets.CodeBase;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private MaxSpawnPointPosition _ground;
    [SerializeField] private float _offset;
    [SerializeField] private LayerMask _interferencesMask;
    [SerializeField] private float _duration;

    private Resource _resource;    
    private bool _isGameWorked;

    private void Awake()
    {        
        _resource = Resources.Load(PrefabsPath.ResourcePath).GetComponent<Resource>();        
    }

    private void Start()
    {
        _isGameWorked = true;

        StartCoroutine(SpawnResource());
    }    
    
    private IEnumerator SpawnResource()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_duration);
        
        while (_isGameWorked)
        {
            Vector3 spawnPosition;
            bool isPositionOccupied = SpawnPointInstaller.TrySetPosition(out spawnPosition, _ground.X, _ground.Z, _resource.Radius + _offset, _interferencesMask);

            if (!isPositionOccupied)
                Instantiate(_resource, spawnPosition, Quaternion.identity);

            yield return waitTime;
        }        
    }    
}