using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ResourcesScaner))]
[RequireComponent(typeof(BaseUnits))]
public class SenderForResources : MonoBehaviour
{
    [SerializeField] private float _resourceCollectionDuration;
    
    private Coroutine _findResourceJob;
    private Coroutine _findFreeUnitJob;

    private Unit _currentFreeUnit;
    private float _radius;
    private Resource _currentTarget;
    private ResourcesScaner _resourcesScaner;
    private BaseUnits _baseUnits;
    private CollectingResources _collectingResources;
    private float _targetRadius;
    private bool _isCollectResource;

    public event UnityAction ResourceCollected;
    public event UnityAction<Unit> UnitReleased;

    private void Awake()
    {
        _radius = GetComponentInChildren<CapsuleCollider>().radius;
        _resourcesScaner = GetComponent<ResourcesScaner>();
        _baseUnits = GetComponent<BaseUnits>();
    }    

    public IEnumerator TrySendUnitForNearestResource()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_resourceCollectionDuration);
        _isCollectResource = true;

        while (_isCollectResource)
        {
            yield return waitTime;

            yield return _findResourceJob = StartCoroutine(FindResource());

            yield return _findFreeUnitJob = StartCoroutine(FindFreeUnit());

            _collectingResources = _currentFreeUnit.GetComponent<CollectingResources>();
            _collectingResources.ResourceDelivered += OnResourceDelivered;

            StartCoroutine(_collectingResources.Collect(_currentTarget, _targetRadius, transform.position, _radius));
        }

        StopCoroutine(_findResourceJob);
        StopCoroutine(_findFreeUnitJob);        
    }

    private IEnumerator FindFreeUnit()
    {
        while (!_baseUnits.TryGetFreeUnit(out _currentFreeUnit))
        {
            yield return null;
        }        
    }

    private IEnumerator FindResource()
    {
        while (!_resourcesScaner.TryGetTarget(out _currentTarget, out _targetRadius))
        {
            yield return null;
        }
    }

    private void OnResourceDelivered(Unit acceptedUnit)
    {
        CollectingResources collectingResources = acceptedUnit.GetComponent<CollectingResources>();
        collectingResources.ResourceDelivered -= OnResourceDelivered;        

        UnitReleased?.Invoke(acceptedUnit);
        ResourceCollected?.Invoke();
    }
}