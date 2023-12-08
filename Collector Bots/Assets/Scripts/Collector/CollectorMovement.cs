using System.Collections;
using UnityEngine;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;
    private Vector3 _targetPosition;

    private void OnDisable()
    {
        StopCoroutine(Move());
    }

    public void StartMoving()
    {
        StartCoroutine(Move());
    }

    private void StopMoving()
    {
        StopCoroutine(Move());
    }

    private void RotateToTarget()
    {
        transform.forward = _target.position - transform.position;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
       _targetPosition = new Vector3(_target.position.x, transform.position.y, _target.position.z);
        RotateToTarget();
    }

    public void UnsetTarget()
    {
        _target = null;
        StopMoving();
    }

    private IEnumerator Move()
    {
        WaitForEndOfFrame waitTime = new WaitForEndOfFrame();
        bool isMoving = true;

        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            yield return waitTime;
        }
    }
}
