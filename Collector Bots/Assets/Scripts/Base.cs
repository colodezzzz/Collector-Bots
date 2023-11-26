using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Base : MonoBehaviour
{
    public int ResourcesAmount { get; private set; }

    [SerializeField] private Transform _collectorsPlace;
    [SerializeField] private Collector _collectorTemplate;

    [SerializeField] private Vector3 _leftUpAreaPoint;
    [SerializeField] private Vector3 _rightDownAreaPoint;
    [SerializeField] private LayerMask _resourcesLayer;
    [SerializeField] private Text _resourcesAmountText;

    [SerializeField] private float _checkDelay;

    private Transform[] _collectorsPlaces;
    private Collector[] _collectors;

    public void GetResource(Resource resource)
    {
        ResourcesAmount++;
        _resourcesAmountText.text = ResourcesAmount.ToString();
        Destroy(resource.gameObject);
    }

    private void Start()
    {
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
            _collectors[i].HomePlace = _collectorsPlaces[i];
        }

        StartCoroutine(CheckResources());
    }

    private IEnumerator CheckResources()
    {
        bool isWorking = enabled;

        while (isWorking)
        {
            yield return new WaitForSeconds(_checkDelay);

            Vector3 areaCenter = (_rightDownAreaPoint - _leftUpAreaPoint) / 2;
            areaCenter.y = 0f;

            float areaHalfWidth = Mathf.Abs(_rightDownAreaPoint.x - _leftUpAreaPoint.x);
            float areaHalfLength = Mathf.Abs(_rightDownAreaPoint.z - _leftUpAreaPoint.z);
            float areaHeight = 0.5f;


            Collider[] resources = Physics.OverlapBox(areaCenter, new Vector3(areaHalfWidth, areaHeight, areaHalfLength), transform.rotation, _resourcesLayer);

            int resourceIndex = 0;

            if (resources.Length > 0)
            {
                foreach (var collector in _collectors)
                {
                    if (collector.Target == null)
                    {
                        collector.StartWorking(resources[resourceIndex++].transform, _resourcesLayer);
                    }
                }
            }
        }
    }
}
