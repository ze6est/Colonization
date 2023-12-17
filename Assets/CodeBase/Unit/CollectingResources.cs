using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementToTarget))]
[RequireComponent(typeof(Unit))]
public class CollectingResources : MonoBehaviour
{
    [SerializeField] float _offsetToTargetForAction;
    [SerializeField] float _liftingHeight;

    private MovementToTarget _movement;
    private Unit _unit;
    private float _unitRadius;

    public event UnityAction<Unit> ResourceDelivered;

    private void Awake()
    {
        _movement = GetComponent<MovementToTarget>();
        _unit = GetComponent<Unit>();
        _unitRadius = _unit.Radius;
    }

    public IEnumerator Collect(Resource resource, float resourceRadius, Vector3 baseUnitsPosition, float baseUnitsRadius)
    {
        float minDistanceToResourceForAction = resourceRadius + _unitRadius + _offsetToTargetForAction;        

        yield return StartCoroutine(_movement.MoveToTarget(resource.transform.position, minDistanceToResourceForAction));

        Raise(resource);

        float minDistanceToBaseUnitsForAction = minDistanceToResourceForAction + resourceRadius + baseUnitsRadius;
        
        yield return StartCoroutine(_movement.MoveToTarget(baseUnitsPosition, minDistanceToBaseUnitsForAction));

        Destroy(resource.gameObject);

        ResourceDelivered?.Invoke(_unit);
    }

    private void Raise(Resource resource)
    {
        resource.transform.position += new Vector3(0, _liftingHeight, 0);
        resource.transform.parent = transform;        
    }
}