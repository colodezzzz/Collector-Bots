using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    private const int SetFlagMouseButton = 0;
    private const int UnsetFlagMouseButton = 1;

    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private float _maxRayDistance;
    [SerializeField] private LayerMask _baseLayers;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Material _changedBaseMaterial;
    
    private Camera _camera;
    private Flag _flag;
    private Base _base;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(SetFlagMouseButton))
        {
            CheckPressing();
        }
        else if (Input.GetMouseButtonDown(UnsetFlagMouseButton))
        {
            UnsetBase();
        }
    }

    private void CheckPressing()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit, _maxRayDistance, _baseLayers))
        {
            SetBase();
        }
        else if (Physics.Raycast(_ray, out _hit, _maxRayDistance, _groundLayers))
        {
            SetFlag();
        }
    }

    private void SetFlag()
    {
        if (_base != null)
        {
            _flag = Instantiate(_flagPrefab, _hit.point, _flagPrefab.transform.rotation);
            _base.SetFlag(_flag);
            UnsetBase();
        }
    }

    private void SetBase()
    {
        Base newBase = _hit.collider.GetComponentInParent<Base>();

        if (newBase == _base)
        {
            UnsetBase();
        }
        else
        {
            UnsetBase();

            if (newBase.CurrentFlag == null)
            {
                _base = newBase;
                _base.ChangeMaterial(_changedBaseMaterial);
            }
        }
    }

    private void UnsetBase()
    {
        if (_base != null)
        {
            _base.ChangeMaterial();
            _base = null;
        }
    }
}
