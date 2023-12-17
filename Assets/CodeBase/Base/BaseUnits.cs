using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitSpawner))]
[RequireComponent(typeof(ResourcesScaner))]
public class BaseUnits : MonoBehaviour
{
    [SerializeField] private float _resourceCollectionDuration;

    private float _radius;
    private UnitSpawner _unitSpawner;
    private ResourcesScaner _resourcesScaner;
    private List<Unit> _freeUnits = new List<Unit>();
    private List<Unit> _occupiedUnits = new List<Unit>();
    private Unit _currentFreeUnit;
    private Resource _currentTarget;
    private float _targetRadius;
    private bool _isGameWorked;

    public event UnityAction ResourceCollected;

    private void Awake()
    {
        _radius = GetComponentInChildren<CapsuleCollider>().radius;
        _unitSpawner = GetComponent<UnitSpawner>();
        _resourcesScaner = GetComponent<ResourcesScaner>();
    }

    private void OnEnable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
    }

    private void Start()
    {
        _isGameWorked = true;

        StartCoroutine(TrySendUnitForNearestResource());
    }

    private void OnDisable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
    }

    private void OnUnitCreated(Unit unit)
    {
        _freeUnits.Add(unit);
    }

    private IEnumerator TrySendUnitForNearestResource()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_resourceCollectionDuration);

        while (_isGameWorked)
        {
            yield return waitTime;

            yield return StartCoroutine(FindResource());

            yield return StartCoroutine(FindFreeUnit());

            CollectingResources collectingResources = _currentFreeUnit.GetComponent<CollectingResources>();
            collectingResources.ResourceDelivered += OnResourceDelivered;

            StartCoroutine(collectingResources.Collect(_currentTarget, _targetRadius, transform.position, _radius));
        }
    }    

    private IEnumerator FindFreeUnit()
    {
        while(_freeUnits.Count == 0)
        {
            yield return null;
        }

        _currentFreeUnit = _freeUnits.First();
        _freeUnits.Remove(_currentFreeUnit);
        _occupiedUnits.Add(_currentFreeUnit);        
    }

    private IEnumerator FindResource()
    {
        while(!_resourcesScaner.TryGetTarget(out _currentTarget, out _targetRadius))
        {
            yield return null;
        }        
    }

    private void OnResourceDelivered(Unit acceptedUnit)
    {
        Unit freeUnit = _occupiedUnits.First(unit => unit.Number == acceptedUnit.Number);        

        CollectingResources collectingResources = freeUnit.GetComponent<CollectingResources>();
        collectingResources.ResourceDelivered -= OnResourceDelivered;

        _occupiedUnits.Remove(freeUnit);
        _freeUnits.Add(freeUnit);

        ResourceCollected?.Invoke();
    }
}