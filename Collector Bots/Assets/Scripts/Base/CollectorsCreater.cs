using UnityEngine;

public class CollectorsCreater : MonoBehaviour
{
    private Collector[] _collectors;
    private Collector _collectorTemplate;
    private Transform[] _collectorsPlaces;
    private LayerMask _resourcesLayer;
    private int _collectorsAmount;
    private Base _base;

    public bool CanCreateCollector
    {
        get
        {
            return _collectorsAmount < _collectors.Length;
        }
    }

    private void Awake()
    {
        _collectorsAmount = 0;
    }

    public void SetData(Transform collectorsPlace, BaseCreator baseCreater, Base parentBase)
    {
        _base = parentBase;
        _collectorTemplate = baseCreater.CollectorTemplate;
        _resourcesLayer = baseCreater.ResourcesLayer;
        _collectorsPlaces = new Transform[collectorsPlace.childCount];
        _collectors = new Collector[collectorsPlace.childCount];

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectorsPlaces[i] = collectorsPlace.GetChild(i);
        }
    }

    private void CreateCollectors(int amount)
    {
        int createdBots = 0;

        for (int i = 0; i < _collectors.Length; i++)
        {
            if (_collectors[i] == null)
            {
                _collectors[i] = Instantiate(_collectorTemplate, _collectorsPlaces[i].position, transform.rotation);
                _collectors[i].SetData(_collectorsPlaces[i], _resourcesLayer, _base);
                _collectorsAmount++;
                createdBots++;

                if (createdBots == amount)
                {
                    return;
                }
            }
        }
    }
}
