using UnityEngine;

public class Collector : MonoBehaviour
{
    public Transform Target { get; private set; }

    private Transform _homePlace;
    private Base _base;
    private CollectorMovement _collectorMovement;
    private CollectorPicking _collectorPicking;

    private void Awake()
    {
        _collectorMovement = GetComponent<CollectorMovement>();
        _collectorPicking = GetComponent<CollectorPicking>();
    }

    private void Update()
    {
        if (_collectorPicking.CurrentResource != null && Vector3.Distance(transform.position, _homePlace.position) == 0)
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

    public void StartWorking(Transform target)
    {
        _collectorPicking.StartChecking();
        Target = target;
        _collectorMovement.SetTarget(Target);
    }
}
