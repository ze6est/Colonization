using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;

    private Vector3 _targetPosition;
    private Rigidbody _rigidbody;
    private bool _isTargetNotReached;    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isTargetNotReached = true;
    }

    public IEnumerator MoveToTarget(Vector3 targetPosition, float minDistanceToTargetForAction)
    {
        _targetPosition = targetPosition;        

        while (_isTargetNotReached)
        {
            Vector3 currentTargetPosition = new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z);

            StartCoroutine(RotateToTarget(currentTargetPosition));

            ResetTransform();

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, _speed * Time.deltaTime);

            if (Vector3.Distance(_targetPosition, transform.position) <= minDistanceToTargetForAction)            
                _isTargetNotReached = false;            

            yield return null;
        }

        ResetTransform();
        _isTargetNotReached = true;
    }

    private IEnumerator RotateToTarget(Vector3 currentTargetPosition)
    {
        while (_isTargetNotReached)
        {
            Vector3 targetDirection = (currentTargetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _speedRotation * Time.deltaTime);

            yield return null;
        }
    }

    private void ResetTransform()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}