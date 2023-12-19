using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CollectorsCreater))]
[RequireComponent(typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _collectorsPlace;

    [SerializeField] private Text _resourcesAmountText;

    [SerializeField] private MeshRenderer _meshRenderer;

    private CollectorsCreater _collectorsCreater;
    private int _newBasePrice;
    private int _collectorPrice;
    private Material _originalMaterial;
    private Collector _freeCollector;
    private BaseCreator _baseController;
    private BaseBuilder _baseBuilder;
    private int _baseBuilderIndex;

    public int ResourcesAmount { get; private set; }
    public Flag CurrentFlag { get; private set; }
    public bool IsBuildingBase { get; private set; }

    private void Awake()
    {
        IsBuildingBase = false;
        _baseBuilderIndex = -1;
        _collectorsCreater = GetComponent<CollectorsCreater>();
        _baseBuilder = GetComponent<BaseBuilder>();
    }

    private void Update()
    {
        StartBaseActions();
    }

    public void SetData(BaseCreator baseCreater, int startCollectorsAmount)
    {
        _originalMaterial = _meshRenderer.material;
        ResourcesAmount = 0;

        _collectorPrice = baseCreater.CollectorPrice;
        _newBasePrice = baseCreater.NewBasePrice;
        _baseController = baseCreater;

        _collectorsCreater.SetData(_collectorsPlace, baseCreater, this, _baseBuilder);
        _collectorsCreater.CreateCollectors(startCollectorsAmount);

        _baseBuilder.SetData(_collectorsCreater, this, _baseController);
    }

    public bool HasFreeCollector()
    {
        for (int i = 0; i < _baseBuilderIndex; i++)
        {
            if (_collectorsCreater.Collectors[i] != null && _collectorsCreater.Collectors[i].Target == null)
            {
                _freeCollector = _collectorsCreater.Collectors[i];
                return true;
            }
        }

        for (int i = _baseBuilderIndex + 1; i < _collectorsCreater.Collectors.Length; i++)
        {
            if (_collectorsCreater.Collectors[i] != null && _collectorsCreater.Collectors[i].Target == null)
            {
                _freeCollector = _collectorsCreater.Collectors[i];
                return true;
            }
        }

        _freeCollector = null;
        return false;
    }

    public void SendCollector(Resource resource)
    {
        if (_freeCollector != null)
        {
            _freeCollector.StartCollecting(resource.transform, resource);
        }
    }

    public void GetResource(Resource resource)
    {
        ResourcesAmount++;
        Destroy(resource.gameObject);
    }

    public void SetFlag(Flag flag)
    {
        CurrentFlag = flag;
    }

    public void UnsetFlag()
    {
        Destroy(CurrentFlag.gameObject);
        CurrentFlag = null;
    }

    public void ChangeMaterial(Material material = null)
    {
        if (material != null)
        {
            _meshRenderer.material = material;
        }
        else
        {
            _meshRenderer.material = _originalMaterial;
        }
    }

    private void StartBaseActions()
    {
        _resourcesAmountText.text = ResourcesAmount.ToString();

        if (CurrentFlag != null && _baseBuilder.IsBuildingBase == false)
        {
            if (ResourcesAmount >= _newBasePrice)
            {
                ResourcesAmount -= _newBasePrice;
                _baseBuilder.StartBuildBase();
            }
        }
        else if (ResourcesAmount >= _collectorPrice && _collectorsCreater.CanCreateCollector)
        {
            ResourcesAmount -= _collectorPrice;
            _collectorsCreater.CreateCollectors(1);
        }
    }
}
