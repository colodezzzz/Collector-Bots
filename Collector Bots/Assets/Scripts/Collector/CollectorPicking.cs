using UnityEngine;

public class CollectorPicking : MonoBehaviour
{
    public Resource CurrentResource { get; private set; }

    [SerializeField] private Transform _resourcePlace;

    private LayerMask _resourceLayer;
    private Collector _collector;
    private bool _isChecking;
    private Resource _resourceToFind;

    private void Awake()
    {
        _isChecking = false;
    }

    private void Update()
    {
        if (_isChecking && TryGetResource(out Resource resource))
        {
            TakeResource(resource);
        }
    }

    public void SetData(LayerMask resourceLayer, Collector collector)
    {
        _resourceLayer = resourceLayer;
        _collector = collector;
    }

    public void StartChecking(Resource resource)
    {
        _isChecking = true;
        _resourceToFind = resource;
    }

    public Resource GiveResource()
    {
        Resource returnedResource = CurrentResource;
        CurrentResource = null;
        _resourceToFind = null;
        return returnedResource;
    }

    private void TakeResource(Resource resource)
    {
        _isChecking = false;
        CurrentResource = resource;

        CurrentResource.GetComponent<Collider>().enabled = false;
        CurrentResource.transform.parent = transform;
        CurrentResource.transform.position = _resourcePlace.position;

        _collector.GoHome();
    }

    private bool TryGetResource(out Resource resource)
    {
        Collider[] resources = Physics.OverlapBox(_resourcePlace.position, Vector3.one * 0.05f, transform.rotation, _resourceLayer);

        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].GetComponent<Resource>() == _resourceToFind)
            {
                resource = _resourceToFind;
                return true;
            }
        }

        resource = null;
        return false;
    }
}
