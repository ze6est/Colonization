using Unity.VisualScripting;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Ground _ground;
    [SerializeField] private BaseUnits _baseUnits;
    [SerializeField] private MaxSpawnPointPosition _maxSpawnPointPosition;
    [SerializeField] private FlagInstaller _flagInstaller;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private FlagSpawner _flagSpawner;
    [SerializeField] private SenderConstructionBaseUnits _senderConstructionBaseUnits;

    private void Awake()
    {
        _ground = Resources.Load(PrefabsPath.Ground).GetComponent<Ground>();
        _baseUnits = Resources.Load(PrefabsPath.BaseUnits).GetComponent<BaseUnits>();

        _ground = Instantiate(_ground);

        _ground.TryGetComponent(out _maxSpawnPointPosition);
        _ground.TryGetComponent(out _flagInstaller);

        _baseUnits = Instantiate(_baseUnits);
        _baseUnits.TryGetComponent(out _unitSpawner);
        _baseUnits.TryGetComponent(out _flagSpawner);
        _baseUnits.TryGetComponent(out _senderConstructionBaseUnits);

        _unitSpawner.Construct(_maxSpawnPointPosition);
        _flagInstaller.Construct(_flagSpawner);
        _senderConstructionBaseUnits.Construct(_flagInstaller);
    }    
}