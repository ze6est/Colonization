using UnityEngine;

[RequireComponent(typeof(BaseUnits))]
public class ResourceCounter : MonoBehaviour
{
    [SerializeField] ResourceCounterView _view;
    [SerializeField] private int _countResourcesAtStart;

    private BaseUnits _baseUnits;
    private int _countCollectedResources;

    private void Awake()
    {
        _baseUnits = GetComponent<BaseUnits>();
        _countCollectedResources += _countResourcesAtStart;
    }

    private void OnEnable()
    {
        _baseUnits.ResourceCollected += OnResourceCollected;
    }

    private void Start()
    {
        _view.RefreshText(_countCollectedResources);
    }

    private void OnDisable()
    {
        _baseUnits.ResourceCollected -= OnResourceCollected;
    }

    private void OnResourceCollected()
    {
        _countCollectedResources++;
        _view.RefreshText(_countCollectedResources);
    }
}