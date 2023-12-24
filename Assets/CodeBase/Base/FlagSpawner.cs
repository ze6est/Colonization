using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ClickTracker))]
public class FlagSpawner : MonoBehaviour
{
    private ClickTracker _clickTracker;
    private Flag _flag;
    private bool _isFlagReadyToInstalled;
    private bool _isFlagInstalled;

    public event UnityAction FlagReadyToInstalled;

    private void Awake()
    {
        _clickTracker = GetComponent<ClickTracker>();
        _flag = Resources.Load<Flag>(PrefabsPath.Flag);
    }

    private void OnEnable()
    {
        _isFlagInstalled = false;
        _isFlagReadyToInstalled = false;
        _clickTracker.ClickHappened += OnClickHappened;
    }    

    private void OnDisable()
    {
        _clickTracker.ClickHappened -= OnClickHappened;
    }

    public bool TrySpawnFlag(out Flag flag)
    {
        if (_isFlagReadyToInstalled)
        {
            flag = Instantiate(_flag);
            _isFlagInstalled = true;
            _isFlagReadyToInstalled = false;
            return true;
        }
        else
        {
            flag = null;
            return false;
        }
    }

    private void OnClickHappened()
    {
        if(!_isFlagInstalled)
        {
            _isFlagReadyToInstalled = true;
            FlagReadyToInstalled?.Invoke();
        }
    }
}