using UnityEngine;

[RequireComponent(typeof(CollectorMovement), typeof(CollectorPicking))]
public class Collector : MonoBehaviour
{
    private Transform _homePlace;
    private Base _base;
    private BaseBuilder _baseBuilder;
    private CollectorMovement _collectorMovement;
    private CollectorPicking _collectorPicking;
    private bool _isCreatingBase;

    public Transform Target { get; private set; }

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

    public void SetFields(Transform homePlace, LayerMask resourceLayer, Base parent, BaseBuilder baseBuilder)
    {
        _base = parent;
        _baseBuilder = baseBuilder;
        _homePlace = homePlace;
        _collectorPicking.SetFields(resourceLayer, this);
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
        _collectorMovement.StartMoving();
    }

    private void BuildBase()
    {
        _baseBuilder.BuildBase();
        Destroy(gameObject);
    }
}
