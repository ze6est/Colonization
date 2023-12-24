using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ResourceCounter))]
public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private int _countUnitsAtStart;
    [SerializeField] private float _maxSpawnRadius;
    [SerializeField] private float _spawnCheckRadiusUnit;
    [SerializeField] private LayerMask _interferencesMask;

    private MaxSpawnPointPosition _ground;
    private ResourceCounter _counter;
    private Unit _unit;    
    private float _maxSpawnPointPositionX;
    private float _maxSpawnPointPositionZ;    

    public event UnityAction<Unit> UnitCreated;
    public event UnityAction UnitForResourcesCreated;

    public void Construct(MaxSpawnPointPosition ground)
    {
        _ground = ground;
    }

    private void Awake()
    {
        _unit = Resources.Load(PrefabsPath.Unit).GetComponent<Unit>();
        _counter = GetComponent<ResourceCounter>();
    }

    private void OnEnable()
    {
        _counter.ResourcesForCreatingUnitReady += OnResourcesForCreatingUnitReady;        
    }    

    private void Start()
    {
        _maxSpawnPointPositionX = _maxSpawnRadius < _ground.X ? _maxSpawnRadius : _ground.X;
        _maxSpawnPointPositionZ = _maxSpawnRadius < _ground.Z ? _maxSpawnRadius : _ground.Z;

        StartCoroutine(SpawnUnits());
    }

    private void OnDisable()
    {
        _counter.ResourcesForCreatingUnitReady -= OnResourcesForCreatingUnitReady;        
    }

    private void OnResourcesForCreatingUnitReady()
    {
        StartCoroutine(SpawnUnitForResources());
    }

    private IEnumerator SpawnUnits()
    {
        for (int i = 0; i < _countUnitsAtStart; i++)
        {
            yield return StartCoroutine(SpawnUnit());
        }        
    }

    private IEnumerator SpawnUnit()
    {
        bool isPositionOccupied = true;

        while (isPositionOccupied)
        {
            Vector3 spawnPosition;
            isPositionOccupied = SpawnPointInstaller.TrySetPosition(out spawnPosition, _maxSpawnPointPositionX, _maxSpawnPointPositionZ, _spawnCheckRadiusUnit, _interferencesMask);

            if (!isPositionOccupied)
            {
                Unit unit = Instantiate(_unit, spawnPosition, Quaternion.identity);
                UnitCreated?.Invoke(unit);
            }

            yield return null;
        }        
    }

    private IEnumerator SpawnUnitForResources()
    {
        yield return StartCoroutine(SpawnUnit());
        UnitForResourcesCreated?.Invoke();
    }
}