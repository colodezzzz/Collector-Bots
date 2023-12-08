using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private const int StartCollectorsAmount = 1;

    public Collector CollectorTemplate { get; private set; }
    public int CollectorPrice { get; private set; }
    public LayerMask ResourcesLayer { get; private set; }
    public int NewBasePrice { get; private set; }

    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _firstBasePlace;

    [SerializeField] private LayerMask _resourcesLayer;

    [SerializeField] private Collector _collectorTemplate;
    [SerializeField] private int _collectorPrice;
    [SerializeField] private int _firstBaseCollectorsAmount;

    [SerializeField] private int _newBasePrice;
    
    private Queue<Resource> _resources;
    private List<Base> _bases;

    private void Awake()
    {
        _resources = new Queue<Resource>();
        _bases = new List<Base>();

        CollectorTemplate = _collectorTemplate;
        CollectorPrice = _collectorPrice;
        ResourcesLayer = _resourcesLayer;
        NewBasePrice = _newBasePrice;
    }

    private void Start()
    {
        CreateBase(_firstBasePlace.position, _firstBaseCollectorsAmount);
    }

    private void Update()
    {
        SendCollectorToResource();
    }

    public Base CreateBase(Vector3 basePosition, int collectorsStartAmount = StartCollectorsAmount)
    {
        Base newBase = Instantiate(_basePrefab, basePosition, Quaternion.identity);

        Vector3 baseDirection = transform.position - basePosition;
        baseDirection.y = 0;
        newBase.transform.forward = baseDirection;

        newBase.SetData(this, collectorsStartAmount);

        _bases.Add(newBase);

        return newBase;
    }

    public void AddResourceToQueue(Resource resource)
    {
        _resources.Enqueue(resource);
    }

    private void SendCollectorToResource()
    {
        if (_resources.Count > 0)
        {
            foreach (Base currentBase in _bases)
            {
                if (currentBase.HasFreeCollector())
                {
                    currentBase.SendCollector(_resources.Dequeue());
                    return;
                }
            }
        }
    }
}
