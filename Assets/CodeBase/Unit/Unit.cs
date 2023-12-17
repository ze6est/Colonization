using UnityEngine;

public class Unit : MonoBehaviour
{
    private static int Id;    

    private float _radius;

    public int Number { get; private set; }
    public float Radius { get { return _radius; } }

    private void Awake()
    {
        Id++;
        Number = Id;
        _radius = GetComponentInChildren<CapsuleCollider>().radius;        
    }    
}