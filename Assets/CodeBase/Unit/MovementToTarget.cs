using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovementToTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;

    private Vector3 _targetPosition;
    private Coroutine _moveToTargetJob;
    private Coroutine _rotateToTargetJob;
    private bool _isTargetNotReached;

    public event UnityAction TargetReached;

    private void Start()
    {
        _isTargetNotReached = true;
        _targetPosition = new Vector3(8, 0, 8);
        _moveToTargetJob = StartCoroutine(MoveToTarget(_targetPosition, 1.1f));
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition, float minDistanceToTargetForAction)
    {
        _targetPosition = targetPosition;        

        while (_isTargetNotReached)
        {
            Vector3 currentTargetPosition = new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z);

            _rotateToTargetJob = StartCoroutine(RotateToTarget(currentTargetPosition));

            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, _speed * Time.deltaTime);            

            if (Vector3.Distance(_targetPosition, transform.position) <= minDistanceToTargetForAction)
            {
                _isTargetNotReached = false;

                if (_moveToTargetJob != null)
                {
                    StopCoroutine(_rotateToTargetJob);
                    StopCoroutine(_moveToTargetJob);
                }

                TargetReached?.Invoke();
            }

            yield return null;
        }
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
}