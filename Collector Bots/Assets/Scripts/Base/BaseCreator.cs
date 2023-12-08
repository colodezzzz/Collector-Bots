using System.Collections.Generic;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    private const int StartCollectorsAmount = 1;

    [SerializeField] private Base _basePrefab;
    [SerializeField] private int _basePrice;

    [SerializeField] private Transform _firstBasePlace;
    [SerializeField] private int _firstBaseCollectorsAmount;

    [SerializeField] private LayerMask _resourcesLayer;

    [SerializeField] private Collector _collectorTemplate;
    [SerializeField] private int _collectorPrice;

    public Collector CollectorTemplate { get; private set; }
    public int CollectorPrice { get; private set; }
    public LayerMask ResourcesLayer { get; private set; }
    public int NewBasePrice { get; private set; }
    public List<Base> Bases { get; private set; }

    private void Awake()
    {
        Bases = new List<Base>();

        CollectorTemplate = _collectorTemplate;
        CollectorPrice = _collectorPrice;
        ResourcesLayer = _resourcesLayer;
        NewBasePrice = _basePrice;
    }

    private void Start()
    {
        CreateBase(_firstBasePlace.position, _firstBaseCollectorsAmount);
    }

    public Base CreateBase(Vector3 basePosition, int collectorsStartAmount = StartCollectorsAmount)
    {
        Base newBase = Instantiate(_basePrefab, basePosition, Quaternion.identity);

        Vector3 baseDirection = transform.position - basePosition;
        baseDirection.y = 0;
        newBase.transform.forward = baseDirection;

        newBase.SetData(this, collectorsStartAmount);

        Bases.Add(newBase);

        return newBase;
    }
}
