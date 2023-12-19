using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner))]
[RequireComponent(typeof(SenderForResources))]
public class BaseUnits : MonoBehaviour
{    
    private UnitSpawner _unitSpawner;
    private SenderForResources _senderForResources;
    private List<Unit> _freeUnits = new List<Unit>();
    private List<Unit> _occupiedUnits = new List<Unit>();    
    private Coroutine _collectResourcesJob;      

    private void Awake()
    {        
        _unitSpawner = GetComponent<UnitSpawner>();
        _senderForResources = GetComponent<SenderForResources>();
    }

    private void OnEnable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
        _senderForResources.UnitReleased += OnUnitReleased;
    }    

    private void Start()
    {        
        _collectResourcesJob = StartCoroutine(_senderForResources.TrySendUnitForNearestResource());        
    }

    private void OnDisable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
    }

    private void OnUnitCreated(Unit unit)
    {
        _freeUnits.Add(unit);
    }

    private void OnUnitReleased(Unit releasedUnit)
    {
        Unit freeUnit = _occupiedUnits.First(unit => unit.Number == releasedUnit.Number);

        _occupiedUnits.Remove(freeUnit);
        _freeUnits.Add(freeUnit);
    }

    public bool TryGetFreeUnit(out Unit _currentFreeUnit)
    {
        if(_freeUnits.Count == 0)
        {
            _currentFreeUnit = null;
            return false;
        }

        _currentFreeUnit = _freeUnits.First();
        _freeUnits.Remove(_currentFreeUnit);
        _occupiedUnits.Add(_currentFreeUnit);

        return true;
    }
}