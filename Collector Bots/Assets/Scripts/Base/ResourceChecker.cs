using UnityEngine;

[RequireComponent(typeof(BaseController))]
public class ResourceChecker : MonoBehaviour
{
    private BaseController _baseController;

    private void Awake()
    {
        _baseController = GetComponent<BaseController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
        {
            _baseController.AddResourceToQueue(resource);
        }
    }
}
