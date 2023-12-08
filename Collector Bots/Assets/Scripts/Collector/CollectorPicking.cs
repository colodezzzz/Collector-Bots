using UnityEngine;

public class CollectorPicking : MonoBehaviour
{
    [SerializeField] private Transform _resourcePlace;
    [SerializeField] private float _checkBoxScale = 0.05f;

    private LayerMask _resourceLayer;
    private Collector _collector;
    private bool _isChecking;
    private Resource _resourceToFind;

    public Resource CurrentResource { get; private set; }

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

        CurrentResource.Taked(transform, _resourcePlace.position);

        _collector.GoHome();
    }

    private bool TryGetResource(out Resource resource)
    {
        Vector3 checkBoxSize = Vector3.one * _checkBoxScale;
        Collider[] resources = Physics.OverlapBox(_resourcePlace.position, checkBoxSize, transform.rotation, _resourceLayer);

        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].TryGetComponent<Resource>(out resource) && resource == _resourceToFind)
            {
                return true;
            }
        }

        resource = null;
        return false;
    }
}
