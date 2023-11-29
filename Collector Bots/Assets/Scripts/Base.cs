using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    public int ResourcesAmount { get; private set; }

    [SerializeField] private Transform _collectorsPlace;
    [SerializeField] private Collector _collectorTemplate;

    [SerializeField] private LayerMask _resourcesLayer;
    [SerializeField] private Text _resourcesAmountText;

    private Transform[] _collectorsPlaces;
    private Collector[] _collectors;
    private Queue<Resource> _resources;

    private void Start()
    {
        _resources = new Queue<Resource>();
        ResourcesAmount = 0;
        _collectorsPlaces = new Transform[_collectorsPlace.childCount];
        _collectors = new Collector[_collectorsPlace.childCount];

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectorsPlaces[i] = _collectorsPlace.GetChild(i);
        }

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectors[i] = Instantiate(_collectorTemplate, _collectorsPlaces[i].position, Quaternion.identity);
            _collectors[i].SetData(_collectorsPlaces[i], _resourcesLayer, this);
        }
    }

    private void Update()
    {
        if (_resources.Count > 0)
        {
            foreach (var collector in _collectors)
            {
                if (collector.Target == null)
                {
                    collector.StartWorking(_resources.Dequeue().transform);
                }

                if (_resources.Count == 0)
                {
                    break;
                }
            }
        }
    }

    public void GetResource(Resource resource)
    {
        ResourcesAmount++;
        _resourcesAmountText.text = ResourcesAmount.ToString();
        Destroy(resource.gameObject);
    }

    public void AddResourceToQueue(Resource resource)
    {
        _resources.Enqueue(resource);
    }
}
