using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseController))]
public class ResourceChecker : MonoBehaviour
{
    [SerializeField] private float _checkAreaScale;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _addingResourcesTime;

    private BaseController _baseController;

    private void Awake()
    {
        _baseController = GetComponent<BaseController>();
    }

    private void Start()
    {
        StartCoroutine(AddNewResources());
    }

    private IEnumerator AddNewResources()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_addingResourcesTime);
        bool isChecking = true;

        while (isChecking)
        {
            Vector3 checkAreaSize = Vector3.one * _checkAreaScale;
            Collider[] resources = Physics.OverlapBox(transform.position, checkAreaSize, transform.rotation, _resourceLayer);

            foreach (Collider resource in resources)
            {
                Resource currentResource = resource.GetComponent<Resource>();

                if (currentResource.IsMarked == false)
                {
                    currentResource.Mark();
                    _baseController.AddResourceToQueue(currentResource);
                }
            }

            yield return waitTime;
        }
    }
}
