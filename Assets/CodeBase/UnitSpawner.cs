using System.Collections;
using Assets.CodeBase;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private MaxSpawnPointPosition _ground;
    [SerializeField] private int _countUnits;
    [SerializeField] private float _maxSpawnRadius;
    [SerializeField] private float _offset;
    [SerializeField] private LayerMask _interferencesMask;

    private Unit _unit;
    private Coroutine _spawnUnitsJob;
    private float _maxSpawnPointPositionX;
    private float _maxSpawnPointPositionZ;

    private void Awake()
    {
        _unit = Resources.Load(PrefabsPath.UnitPath).GetComponent<Unit>();
    }

    private void Start()
    {
        _maxSpawnPointPositionX = _maxSpawnRadius < _ground.X ? _maxSpawnRadius : _ground.X;
        _maxSpawnPointPositionZ = _maxSpawnRadius < _ground.Z ? _maxSpawnRadius : _ground.Z;

        _spawnUnitsJob =StartCoroutine(SpawnUnits());
    }

    private void OnDisable()
    {
        if (_spawnUnitsJob != null)
            StopCoroutine(_spawnUnitsJob);
    }

    private IEnumerator SpawnUnits()
    {
        for (int i = 0; i < _countUnits; i++)
        {
            yield return StartCoroutine(SpawnUnit());
        }
    }

    private IEnumerator SpawnUnit()
    {
        bool isPositionOccupied = true;        

        while (isPositionOccupied == true)
        {
            Vector3 spawnPosition;
            isPositionOccupied = SpawnPointInstaller.TrySetPosition(out spawnPosition, _maxSpawnPointPositionX, _maxSpawnPointPositionZ, _unit.Radius + _offset, _interferencesMask);

            if(!isPositionOccupied)            
                Instantiate(_unit, spawnPosition, Quaternion.identity);            

            yield return null;
        }        
    }    
}