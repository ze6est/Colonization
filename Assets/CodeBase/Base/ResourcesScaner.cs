using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesScaner : MonoBehaviour
{
    private const string FoundResource = "FoundResource";

    [SerializeField] private float _scanRadius;
    [SerializeField] private float _duration;
    [SerializeField] private LayerMask _newResourcesMask;    

    private Dictionary<Vector3, Resource> _foundResources = new Dictionary<Vector3, Resource>();    

    private bool _isGameWorked;

    private void Start()
    {
        _isGameWorked = true;

        StartCoroutine(ScanResource());
    }

    public bool TryGetTarget(out Resource target, out float resourceRadius)
    {
        if(_foundResources.Count == 0)
        {
            target = null;
            resourceRadius = 0;
            return false;
        }
        else
        {
            float minDistance = int.MaxValue;
            target = null;
            resourceRadius = 0;            
            
            Vector3 currentPosition = Vector3.zero;

            foreach (Vector3 targetPosition in _foundResources.Keys)
            {
                float distanceToTarget = Vector3.Distance(targetPosition, transform.position);

                if (minDistance < distanceToTarget)
                    continue;
                else
                {
                    minDistance = distanceToTarget;
                    target = _foundResources[targetPosition];
                    currentPosition = targetPosition;                    
                }
            }
            
            resourceRadius = _foundResources[currentPosition].Radius;
            _foundResources.Remove(currentPosition);            

            return true;
        }        
    }

    private IEnumerator ScanResource()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_duration);        

        while (_isGameWorked)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _newResourcesMask);
            int length = colliders.Length;            

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    Resource resource = colliders[i].GetComponentInParent<Resource>();

                    Vector3 position = resource.transform.position;

                    _foundResources.Add(position, resource);

                    colliders[i].gameObject.layer = LayerMask.NameToLayer(FoundResource);
                    resource.gameObject.layer = LayerMask.NameToLayer(FoundResource);                    
                }
            }

            yield return waitTime;
        }
    }
}