using UnityEngine;

public class CollectorsCreater : MonoBehaviour
{
    private Collector _collectorTemplate;
    private Transform[] _collectorsPlaces;
    private LayerMask _resourcesLayer;
    private int _collectorsAmount;
    private Base _base;
    private BaseBuilder _baseBuilder;

    public Collector[] Collectors { get; private set; }

    public bool CanCreateCollector => _collectorsAmount < Collectors.Length;

    private void Awake()
    {
        _collectorsAmount = 0;
    }

    public void SetFields(Transform collectorsPlace, BaseCreator baseCreater, Base parentBase, BaseBuilder baseBuilder)
    {
        _base = parentBase;
        _baseBuilder = baseBuilder;
        _collectorTemplate = baseCreater.CollectorTemplate;
        _resourcesLayer = baseCreater.ResourcesLayer;
        _collectorsPlaces = new Transform[collectorsPlace.childCount];
        Collectors = new Collector[collectorsPlace.childCount];

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectorsPlaces[i] = collectorsPlace.GetChild(i);
        }
    }

    public void CreateCollectors(int amount)
    {
        int createdBots = 0;

        for (int i = 0; i < Collectors.Length; i++)
        {
            if (Collectors[i] == null)
            {
                Collectors[i] = Instantiate(_collectorTemplate, _collectorsPlaces[i].position, transform.rotation);
                Collectors[i].SetFields(_collectorsPlaces[i], _resourcesLayer, _base, _baseBuilder);
                _collectorsAmount++;
                createdBots++;

                if (createdBots == amount)
                {
                    return;
                }
            }
        }
    }

    public void DeleteCollector(int collectorIndexToDelete)
    {
        _collectorsAmount--;
        Collectors[collectorIndexToDelete] = null;
    }
}
