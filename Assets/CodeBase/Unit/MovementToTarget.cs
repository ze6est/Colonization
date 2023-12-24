using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class MovementToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    
    private Rigidbody _rigidbody;    
    private bool _isTargetNotReached;

    public event UnityAction TargetReached;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isTargetNotReached = true;
    }

    public IEnumerator MoveToTarget(Vector3 targetPosition, float minDistanceToTargetForAction)
    {
        while (_isTargetNotReached)
        {
            Vector3 currentTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

            RotateToTarget(currentTargetPosition);

            ResetTransform();

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, _speed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, transform.position) <= minDistanceToTargetForAction)
            {
                TargetReached?.Invoke();
                _isTargetNotReached = false;                
            }

            yield return null;
        }

        ResetTransform();
        _isTargetNotReached = true;
    }

    private void RotateToTarget(Vector3 currentTargetPosition)
    {
        Vector3 targetDirection = (currentTargetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _speedRotation * Time.deltaTime);
    }

    private void ResetTransform()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}