using UnityEngine;

public class ResourceChecker : MonoBehaviour
{
    private Base _base;

    private void Awake()
    {
        _base = GetComponentInParent<Base>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
        {
            _base.AddResourceToQueue(resource);
        }
    }
}
