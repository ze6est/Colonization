using UnityEngine;

public class Flag : MonoBehaviour
{
    private float _radius;
    
    public float Radius { get { return _radius; } }

    private void Awake()
    {        
        _radius = GetComponentInChildren<CapsuleCollider>().radius;
    }
}