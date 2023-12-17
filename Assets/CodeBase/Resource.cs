using UnityEngine;

public class Resource : MonoBehaviour
{
    private float _radius;

    public float Radius { get { return _radius; } }

    private void Awake()
    {
        _radius = GetComponentInChildren<SphereCollider>().radius;
    }
}