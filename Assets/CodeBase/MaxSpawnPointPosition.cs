using UnityEngine;

public class MaxSpawnPointPosition : MonoBehaviour
{
    [SerializeField] private float _deadZone;

    public float X { get; private set; }
    public float Z { get; private set; }

    private void Awake()
    {
        Vector3 planeSize = transform.GetComponent<Renderer>().bounds.size;
        X = planeSize.x / 2 - _deadZone;
        Z = planeSize.z / 2 - _deadZone;        
    }
}