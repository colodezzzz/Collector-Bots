using UnityEngine;

public class Collector : MonoBehaviour
{
    public Transform Target { get; private set; }

    [SerializeField] private float _speed;
    [SerializeField] private Transform _resourcePlace;

    private Resource _resource;
    private LayerMask _resourceLayer;
    private Base _base;
    private Transform _homePlace;

    public void SetData(Transform homePlace, Base parent)
    {
        _base = parent;
        _homePlace = homePlace;
    }

    public void StartWorking(Transform target, LayerMask resourceLayer)
    {
        Target = target;
        _resourceLayer = resourceLayer;
        RotateToTarget();
    }

    public Resource GiveResource()
    {
        Resource takedResource = _resource;
        _resource = null;
        return takedResource;
    }

    private void Update()
    {
        if (Target != null)
        {
            Vector3 targetPosition = new Vector3(Target.position.x, transform.position.y, Target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (_resource == null && TryGetResource(out Resource resource))
            {
                TakeResource(resource);
            }

            if (_resource != null && transform.position == _homePlace.position)
            {
                Target = null;
                _base.GetResource(GiveResource());
            }
        }
    }

    private void TakeResource(Resource resource)
    {
        _resource = resource;

        _resource.GetComponent<Collider>().enabled = false;
        _resource.transform.parent = transform;
        _resource.transform.position = _resourcePlace.position;

        Target = _homePlace;
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        transform.forward = Target.position - transform.position;
    }

    private bool TryGetResource(out Resource resource)
    {
        Collider[] resources = Physics.OverlapBox(_resourcePlace.position, Vector3.one * 0.05f, transform.rotation, _resourceLayer);

        if (resources.Length > 0)
        {
            resource = resources[0].GetComponent<Resource>();
            return true;
        }

        resource = null;
        return false;
    }
}
