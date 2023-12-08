using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCreator))]
public class ResourceChecker : MonoBehaviour
{
    [SerializeField] private float _checkAreaScale;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _addingResourcesTime;

    private BaseCreator _baseCreator;
    private Queue<Resource> _resources;
    private Coroutine _addResourcesCoroutine;

    private void Awake()
    {
        _baseCreator = GetComponent<BaseCreator>();
    }

    private void Start()
    {
        _resources = new Queue<Resource>();
        _addResourcesCoroutine = StartCoroutine(AddNewResources());
    }

    private void Update()
    {
        SendCollectorToResource();
    }

    private void OnDisable()
    {
        if (_addResourcesCoroutine != null)
        {
            StopCoroutine(_addResourcesCoroutine);
        }
    }

    private void AddResourceToQueue(Resource resource)
    {
        _resources.Enqueue(resource);
    }

    private void SendCollectorToResource()
    {
        if (_resources.Count > 0)
        {
            foreach (Base currentBase in _baseCreator.Bases)
            {
                if (currentBase.HasFreeCollector())
                {
                    currentBase.SendCollector(_resources.Dequeue());
                    return;
                }
            }
        }
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
                    AddResourceToQueue(currentResource);
                }
            }

            yield return waitTime;
        }
    }
}
