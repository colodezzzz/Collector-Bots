using UnityEngine;

[RequireComponent(typeof(Collider))] 
public class Resource : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Taked(Transform parent, Vector3 position)
    {
        _collider.enabled = false;
        transform.parent = parent;
        transform.position = position;
    }
}
