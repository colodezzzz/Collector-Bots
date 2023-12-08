using UnityEngine;

[RequireComponent(typeof(CollectorMovement), typeof(CollectorPicking))]
public class Collector : MonoBehaviour
{
    public Transform Target { get; private set; }

    private Transform _homePlace;
    private Base _base;
    private CollectorMovement _collectorMovement;
    private CollectorPicking _collectorPicking;
    private bool _isCreatingBase;

    private void Awake()
    {
        _isCreatingBase = false;
        _collectorMovement = GetComponent<CollectorMovement>();
        _collectorPicking = GetComponent<CollectorPicking>();
    }

    private void Update()
    {
        if (_isCreatingBase)
        {
            Vector3 collectorPosition = transform.position;
            collectorPosition.y = 0;

            Vector3 targetPosition = Target.position;
            targetPosition.y = 0;

            if (Vector3.Distance(collectorPosition, targetPosition) == 0)
            {
                BuildBase();
            }
        }
        else if (_collectorPicking.CurrentResource != null && Vector3.Distance(transform.position, _homePlace.position) == 0)
        {
            _base.GetResource(_collectorPicking.GiveResource());
            Target = null;
            _collectorMovement.UnsetTarget();
        }
    }

    public void GoHome()
    {
        Target = _homePlace;
        _collectorMovement.SetTarget(Target);
    }

    public void SetData(Transform homePlace, LayerMask resourceLayer, Base parent)
    {
        _base = parent;
        _homePlace = homePlace;
        _collectorPicking.SetData(resourceLayer, this);
    }

    public void StartCollecting(Transform target, Resource resource)
    {
        _collectorPicking.StartChecking(resource);
        SetTarget(target);
    }

    public void StartBuildBase(Transform target)
    {
        _isCreatingBase = true;
        SetTarget(target);
    }

    private void SetTarget(Transform target)
    {
        Target = target;
        _collectorMovement.SetTarget(Target);
    }

    private void BuildBase()
    {
        _base.BuildBase(transform.position);
        Destroy(gameObject);
    }
}
