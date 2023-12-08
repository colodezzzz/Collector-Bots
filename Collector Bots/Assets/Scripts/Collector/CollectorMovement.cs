using System.Collections;
using UnityEngine;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField, TextArea(8, 10)] private string _debugString;

    private Transform _target;
    private Vector3 _targetPosition;
    private int _startMovingAmount = 0;
    private int _stopMovingAmount = 0;

    private void Update()
    {
        if (_target != null)
        {
            Vector3 targetPosition = new Vector3(_target.position.x, transform.position.y, _target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }

        _debugString = $"Collector {this}\nSpeed: {_speed}\nStarts {_startMovingAmount}\nStops {_stopMovingAmount}";
    }

    private void OnDisable()
    {
        StopCoroutine(Move());
    }

    public void StartMoving()
    {
        _startMovingAmount++;
        StartCoroutine(Move());
    }

    private void StopMoving()
    {
        _stopMovingAmount++;
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
