using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SenderForResources))]
[RequireComponent(typeof(UnitSpawner))]
[RequireComponent(typeof(SenderConstructionBaseUnits))]
public class ResourceCounter : MonoBehaviour
{
    private const int CountResourceToCreateUnit = 3;
    private const int CountResourceToCreateBase = 5;

    [SerializeField] ResourceCounterView _view;
    [SerializeField] private int _countResourcesAtStart;

    private SenderForResources _senderForResources;
    private UnitSpawner _unitSpawner;
    private SenderConstructionBaseUnits _senderConstructionBaseUnits;
    private int _countCollectedResources;

    private bool _isCreatingUnits;

    public event UnityAction ResourcesForCreatingUnitReady;
    public event UnityAction ResourcesForBuildingBaseReady;

    public void Construct(ResourceCounterView view, int countResourcesAtStart = 0)
    {
        _view = view;
        _countResourcesAtStart = countResourcesAtStart;
    }

    private void Awake()
    {
        _senderForResources = GetComponent<SenderForResources>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _senderConstructionBaseUnits = GetComponent<SenderConstructionBaseUnits>();
        _countCollectedResources += _countResourcesAtStart;
    }

    private void OnEnable()
    {
        _senderForResources.ResourceCollected += OnResourceCollected;
        _unitSpawner.UnitForResourcesCreated += OnUnitForResourcesCreated;
    }    

    private void Start()
    {        
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
    }

    private void ChangeCountResource(int count)
    {
        _countCollectedResources += count;

        _view.RefreshText(_countCollectedResources);

        if (_countCollectedResources >= CountResourceToCreateUnit)
        {
            ResourcesForCreatingUnitReady?.Invoke();
        }

        if(_countCollectedResources >= CountResourceToCreateBase)
        {
            ResourcesForBuildingBaseReady?.Invoke();
        }
    }
}