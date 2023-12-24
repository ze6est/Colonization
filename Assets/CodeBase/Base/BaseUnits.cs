using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner))]
[RequireComponent(typeof(SenderForResources))]
[RequireComponent(typeof(FlagSpawner))]
[RequireComponent(typeof(SenderConstructionBaseUnits))]
public class BaseUnits : MonoBehaviour
{    
    private UnitSpawner _unitSpawner;
    private SenderForResources _senderForResources;
    private SenderConstructionBaseUnits _senderConstructionBaseUnits;
    private FlagSpawner _flagSpawner;    
    private List<Unit> _freeUnits = new List<Unit>();
    private List<Unit> _occupiedUnits = new List<Unit>();

    public SenderConstructionBaseUnits SenderConstructionBaseUnits => _senderConstructionBaseUnits;

    public void Construct(Unit unit)
    {        
        _freeUnits.Add(unit);
    }

    private void Awake()
    {        
        _unitSpawner = GetComponent<UnitSpawner>();
        _senderForResources = GetComponent<SenderForResources>();
        _senderConstructionBaseUnits = GetComponent<SenderConstructionBaseUnits>();
        _flagSpawner = GetComponent<FlagSpawner>();

        _unitSpawner.enabled = true;
        _senderConstructionBaseUnits.enabled = false;
    }

    private void OnEnable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
        _senderForResources.UnitReleased += OnUnitReleased;
        _flagSpawner.FlagReadyToInstalled += OnFlagReadyToInstalled;        
    }

    private void OnDisable()
    {
        _unitSpawner.UnitCreated += OnUnitCreated;
        _flagSpawner.FlagReadyToInstalled -= OnFlagReadyToInstalled;        
    }

    public bool GetFreeUnit(out Unit _currentFreeUnit)
    {
        if (_freeUnits.Count == 0)
        {
            _currentFreeUnit = null;
            return false;
        }

        _currentFreeUnit = _freeUnits.First();
        _freeUnits.Remove(_currentFreeUnit);
        _occupiedUnits.Add(_currentFreeUnit);

        return true;
    }

    public bool GiveUnitToBuildBase(out Unit _currentFreeUnit)
    {
        if (_freeUnits.Count == 0)
        {
            _currentFreeUnit = null;
            return false;
        }

        _currentFreeUnit = _freeUnits.First();
        _freeUnits.Remove(_currentFreeUnit);        

        return true;
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

    private void OnFlagReadyToInstalled()
    {
        _unitSpawner.enabled = false;
        _senderConstructionBaseUnits.enabled = true;
        _senderConstructionBaseUnits.UnitForBuildingBaseSent += OnUnitForBuildingBaseSent;
    }

    private void OnUnitForBuildingBaseSent()
    {
        _senderConstructionBaseUnits.UnitForBuildingBaseSent -= OnUnitForBuildingBaseSent;
        _senderConstructionBaseUnits.enabled = false;
        _unitSpawner.enabled = true;
    }
}