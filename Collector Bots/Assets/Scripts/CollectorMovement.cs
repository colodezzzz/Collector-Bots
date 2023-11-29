using UnityEngine;

public class CollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;

    private void Update()
    {
        if (_target != null)
        {
            Vector3 targetPosition = new Vector3(_target.position.x, transform.position.y, _target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }

    private void RotateToTarget()
    {
        transform.forward = _target.position - transform.position;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        RotateToTarget();
    }

    public void UnsetTarget()
    {
        _target = null;
    }
}
