using UnityEngine;

public class Unit : MonoBehaviour
{
    private float _radius;

    public float Radius { get { return _radius; } }    

    private void OnValidate()
    {
        _radius = GetComponentInChildren<CapsuleCollider>().radius;
    }
}