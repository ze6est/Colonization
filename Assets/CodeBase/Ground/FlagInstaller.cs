using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ClickTracker))]
public class FlagInstaller : MonoBehaviour
{
    [SerializeField] private float _deathZone = 2f;

    [SerializeField] private FlagSpawner _flagSpawner;
    private ClickTracker _clickTracker;
    private Flag _flag;
    private Mesh _planeMesh;
    private float _extremePointX;
    private float _extremePointZ;
    
    public Flag Flag => _flag;
    public Vector3 FlagPosition { get; private set; }

    public event UnityAction FlagInstalled;
    public event UnityAction<Vector3> PositionChanged;

    public void Construct(FlagSpawner flagSpawner)
    {
        _flagSpawner = flagSpawner;
    }

    private void Awake()
    {
        _clickTracker = GetComponent<ClickTracker>();

        _planeMesh = gameObject.GetComponent<MeshFilter>().mesh;
        Bounds bounds = _planeMesh.bounds;

        _extremePointX = gameObject.transform.localScale.x * bounds.size.x / 2 - _deathZone;        
        _extremePointZ = gameObject.transform.localScale.z * bounds.size.z / 2 - _deathZone;
    }

    private void OnEnable()
    {
        _clickTracker.ClickHappened += OnClickHappened;
    }

    private void OnDisable()
    {
        _clickTracker.ClickHappened -= OnClickHappened;
    }

    private void OnClickHappened()
    {
        if (_flagSpawner.TrySpawnFlag(out Flag flag))
        {
            _flag = flag;

            FlagPosition = _flag.gameObject.transform.position = SetPosition();
            FlagInstalled?.Invoke();
        }
        else
        {
            if(_flag != null)
            {
                FlagPosition = _flag.gameObject.transform.position = SetPosition();
                PositionChanged?.Invoke(FlagPosition);
            }
        }
    }

    private Vector3 SetPosition()
    {
        Vector3 cursorPosition = GetCursorPosition();

        float positionX;
        float positionZ;

        if (Math.Abs(cursorPosition.x) > _extremePointX)
        {
            positionX = _extremePointX;

            if (cursorPosition.x < 0)
                positionX = -positionX;
        }
        else
            positionX = cursorPosition.x;

        if(Math.Abs(cursorPosition.z) > _extremePointZ)
        {
            positionZ = _extremePointZ;

            if(cursorPosition.z < 0)
                positionZ = -positionZ;
        }
        else
            positionZ = cursorPosition.z;

        return new Vector3(positionX, 0, positionZ);
    }

    private Vector3 GetCursorPosition()
    {
        Vector3 cursorPositionOnPlane = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            cursorPositionOnPlane = ray.GetPoint(distance);            
        }

        return cursorPositionOnPlane;
    }
}