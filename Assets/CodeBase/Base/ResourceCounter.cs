using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SenderForResources))]
[RequireComponent(typeof(UnitSpawner))]
public class ResourceCounter : MonoBehaviour
{
    private const int CountResourceToCreateUnit = 3;

    [SerializeField] ResourceCounterView _view;
    [SerializeField] private int _countResourcesAtStart;

    private SenderForResources _senderForResources;
    private UnitSpawner _unitSpawner;
    private int _countCollectedResources;
    private bool _isUnitNotCreated;

    public event UnityAction ResourcesForCreatingUnitReady;

    private void Awake()
    {
        _senderForResources = GetComponent<SenderForResources>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _countCollectedResources += _countResourcesAtStart;
    }

    private void OnEnable()
    {
        _senderForResources.ResourceCollected += OnResourceCollected;
        _unitSpawner.UnitForResourcesCreated += OnUnitForResourcesCreated;
    }    

    private void Start()
    {
        _isUnitNotCreated = true;
        _view.RefreshText(_countCollectedResources);
    }

    private void OnDisable()
    {
        _senderForResources.ResourceCollected -= OnResourceCollected;
        _unitSpawner.UnitForResourcesCreated -= OnUnitForResourcesCreated;
    }

    private void OnResourceCollected()
    {
        ChangeCountResource(1);
    }

    private void OnUnitForResourcesCreated()
    {
        ChangeCountResource(-CountResourceToCreateUnit);
        _isUnitNotCreated = true;
    }

    private void ChangeCountResource(int count)
    {
        _countCollectedResources += count;

        _view.RefreshText(_countCollectedResources);

        if (_isUnitNotCreated && _countCollectedResources >= CountResourceToCreateUnit)
        {
            ResourcesForCreatingUnitReady?.Invoke();
            _isUnitNotCreated = false;
        }        
    }
}