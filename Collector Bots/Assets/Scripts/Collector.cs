using UnityEngine;

public class Collector : MonoBehaviour
{
    public Transform Target { get; private set; }

    [HideInInspector] public Transform HomePlace;

    [SerializeField] private float _speed;
    [SerializeField] private Transform _resourcePlace;

    private GameObject _resource;
    private LayerMask _resourceLayer;

    public void StartWorking(Transform target, LayerMask resourceLayer)
    {
        Target = target;
        _resourceLayer = resourceLayer;
        RotateToTarget();
    }

    public GameObject GiveResource()
    {
        GameObject takedResource = _resource;
        _resource = null;
        return takedResource;
    }

    private void Update()
    {
        if (Target != null)
        {
            Vector3 targetPosition = new Vector3(Target.position.x, transform.position.y, Target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }

        if (_resource == null && TryGetResource(out GameObject resource))
        {
            TakeResource(resource);
        }

        if (_resource != null && transform.position == HomePlace.position)
        {
            Target = null;
        }
    }

    private void TakeResource(GameObject resource)
    {
        _resource = resource;

        _resource.GetComponent<Collider>().enabled = false;
        _resource.transform.parent = transform;
        _resource.transform.position = _resourcePlace.position;

        Target = HomePlace;
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        transform.forward = Target.position - transform.position;
    }

    private bool TryGetResource(out GameObject resource)
    {
        Collider[] resources = Physics.OverlapBox(_resourcePlace.position, Vector3.one * 0.05f, transform.rotation, _resourceLayer);

        if (resources.Length > 0)
        {
            resource = resources[0].gameObject;
            return true;
        }

        resource = null;
        return false;
    }
}
