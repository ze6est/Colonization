using UnityEngine;

[RequireComponent(typeof(SenderForResources))]
public class ResourceCounter : MonoBehaviour
{
    [SerializeField] ResourceCounterView _view;
    [SerializeField] private int _countResourcesAtStart;

    private SenderForResources _senderForResources;
    private int _countCollectedResources;

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
        _countCollectedResources++;
        _view.RefreshText(_countCollectedResources);
    }
}