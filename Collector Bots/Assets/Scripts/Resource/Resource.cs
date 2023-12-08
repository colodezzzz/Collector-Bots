using UnityEngine;

[RequireComponent(typeof(Collider))] 
public class Resource : MonoBehaviour
{
    public bool IsMarked { get; private set; }

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        IsMarked = false;
    }

    public void Taked(Transform parent, Vector3 position)
    {
        _collider.enabled = false;
        transform.parent = parent;
        transform.position = position;
    }

    public void Mark()
    {
        IsMarked = true;
    }
}
