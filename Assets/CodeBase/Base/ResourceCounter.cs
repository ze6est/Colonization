using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SenderForResources))]
public class ResourceCounter : MonoBehaviour
{
    private const int CountResourceToCreateUnit = 3;

    [SerializeField] ResourceCounterView _view;
    [SerializeField] private int _countResourcesAtStart;

    private SenderForResources _senderForResources;    
    private int _countCollectedResources;

    public event UnityAction ResourcesForCreatingUnitReady;

    private void Awake()
    {
        _senderForResources = GetComponent<SenderForResources>();
        _countCollectedResources += _countResourcesAtStart;
    }

    private void OnEnable()
    {
        _senderForResources.ResourceCollected += OnResourceCollected;
    }

    private void Start()
    {
        _view.RefreshText(_countCollectedResources);
    }

    private void OnDisable()
    {
        _senderForResources.ResourceCollected -= OnResourceCollected;
    }

    private void OnResourceCollected()
    {
        ChangeCountResource(1);
    }

    private void ChangeCountResource(int count)
    {
        _countCollectedResources += count;

        _view.RefreshText(_countCollectedResources);

        if (_countCollectedResources >= CountResourceToCreateUnit)
        {
            ResourcesForCreatingUnitReady?.Invoke();
            _countCollectedResources -= CountResourceToCreateUnit;
        }

        _view.RefreshText(_countCollectedResources);
    }
}