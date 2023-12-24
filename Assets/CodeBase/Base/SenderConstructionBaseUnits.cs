using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ResourceCounter))]
public class SenderConstructionBaseUnits : MonoBehaviour
{
    [SerializeField] float _offsetToTargetForAction;

    [SerializeField] private FlagInstaller _flagInstaller;
    private Unit _currentFreeUnit;
    private BaseUnits _baseUnits;
    private ResourceCounter _counter;

    public event UnityAction UnitForBuildingBaseSent;

    public void Construct(FlagInstaller flagInstaller)
    {
        _flagInstaller = flagInstaller;
    }

    private void Awake()
    {
        _baseUnits = GetComponent<BaseUnits>();
        _counter = GetComponent<ResourceCounter>();
    }

    private void OnEnable()
    {
        _flagInstaller.FlagInstalled += OnFlagInstalled;        
    }

    private void OnDisable()
    {
        _flagInstaller.FlagInstalled -= OnFlagInstalled;
    }

    private void OnFlagInstalled()
    {
        _counter.ResourcesForBuildingBaseReady += OnResourcesForBuildingBaseReady;
    }

    private void OnResourcesForBuildingBaseReady()
    {
        StartCoroutine(SendUnitConstructionBaseUnits());
    }

    public IEnumerator SendUnitConstructionBaseUnits()
    {
        yield return StartCoroutine(FindFreeUnit());

        float minDistanceToTargetForAction = _currentFreeUnit.Radius + _flagInstaller.Flag.Radius + _offsetToTargetForAction;        

        BuildingBase buildingBase = _currentFreeUnit.GetComponent<BuildingBase>();
        buildingBase.SetParameters(_flagInstaller, minDistanceToTargetForAction);
        _counter.ResourcesForBuildingBaseReady -= OnResourcesForBuildingBaseReady;

        UnitForBuildingBaseSent?.Invoke();
    }    

    private IEnumerator FindFreeUnit()
    {
        while (!_baseUnits.GiveUnitToBuildBase(out _currentFreeUnit))
        {
            yield return null;
        }
    }    
}