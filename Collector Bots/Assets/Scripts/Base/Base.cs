using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CollectorsCreater))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _collectorsPlace;

    [SerializeField] private Text _resourcesAmountText;

    [SerializeField] private MeshRenderer _meshRenderer;

    private CollectorsCreater _collectorsCreater;
    private LayerMask _resourcesLayer;
    private int _newBasePrice;
    private Collector _collectorTemplate;
    private int _collectorPrice;
    private Transform[] _collectorsPlaces;
    private Collector[] _collectors;
    private Material _originalMaterial;
    private Collector _freeCollector;
    private BaseCreator _baseController;
    private int _baseBuilderIndex;
    private int _collectorsAmount;
    private Coroutine _startBuildingBaseCoroutine;

    public int ResourcesAmount { get; private set; }
    public Flag CurrentFlag { get; private set; }
    public bool IsBuildingBase { get; private set; }

    private void Awake()
    {
        IsBuildingBase = false;
        _baseBuilderIndex = -1;
        _collectorsAmount = 0;
        _collectorsCreater = GetComponent<CollectorsCreater>();
    }

    private void Update()
    {
        StartBaseActions();
    }

    private void OnDestroy()
    {
        StopCoroutines();
    }

    private void OnDisable()
    {
        StopCoroutines();
    }

    public void SetData(BaseCreator baseCreater, int startCollectorsAmount)
    {
        _originalMaterial = _meshRenderer.material;
        ResourcesAmount = 0;
        _collectorsPlaces = new Transform[_collectorsPlace.childCount];
        _collectors = new Collector[_collectorsPlace.childCount];

        _collectorTemplate = baseCreater.CollectorTemplate;
        _collectorPrice = baseCreater.CollectorPrice;
        _resourcesLayer = baseCreater.ResourcesLayer;
        _newBasePrice = baseCreater.NewBasePrice;
        _baseController = baseCreater;

        //_collectorsCreater.SetData(_collectorsPlace, baseCreater, this);

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectorsPlaces[i] = _collectorsPlace.GetChild(i);
        }

        CreateCollectors(startCollectorsAmount);
    }

    public bool HasFreeCollector()
    {
        for (int i = 0; i < _baseBuilderIndex; i++)
        {
            if (_collectors[i] != null && _collectors[i].Target == null)
            {
                _freeCollector = _collectors[i];
                return true;
            }
        }

        for (int i = _baseBuilderIndex + 1; i < _collectors.Length; i++)
        {
            if (_collectors[i] != null && _collectors[i].Target == null)
            {
                _freeCollector = _collectors[i];
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

    public void BuildBase(Vector3 position)
    {
        
        _collectors[_baseBuilderIndex] = null;
        _collectorsAmount--;
        _baseBuilderIndex = -1;
        IsBuildingBase = false;
        UnsetFlag();
        _baseController.CreateBase(position);
    }

    private IEnumerator StartBuildingBase()
    {
        WaitForEndOfFrame waitTime = new WaitForEndOfFrame();
        bool isWorking = true;

        while (isWorking)
        {
            if (TryStartBuildBase())
            {
                break;
            }

            yield return waitTime;
        }
    }

    private void StartBaseActions()
    {
        _resourcesAmountText.text = ResourcesAmount.ToString();

        if (CurrentFlag != null && IsBuildingBase == false)
        {
            if (ResourcesAmount >= _newBasePrice)
            {
                IsBuildingBase = true;
                ResourcesAmount -= _newBasePrice;
                _startBuildingBaseCoroutine = StartCoroutine(StartBuildingBase());
            }
        }
        else if (ResourcesAmount >= _collectorPrice && _collectorsAmount < _collectors.Length)
        {
            ResourcesAmount -= _collectorPrice;
            CreateCollectors(1);
        }
    }

    private bool TryStartBuildBase()
    {
        for (int i = 0; i < _collectors.Length; i++)
        {
            if (_collectors[i] != null && _collectors[i].Target == null)
            {
                _baseBuilderIndex = i;
                _collectors[i].StartBuildBase(CurrentFlag.transform);
                return true;
            }
        }

        return false;
    }

    private void CreateCollectors(int amount)
    {
        int createdBots = 0;

        for (int i = 0; i < _collectors.Length; i++)
        {
            if (_collectors[i] == null)
            {
                _collectors[i] = Instantiate(_collectorTemplate, _collectorsPlaces[i].position, transform.rotation);
                _collectors[i].SetData(_collectorsPlaces[i], _resourcesLayer, this);
                _collectorsAmount++;
                createdBots++;

                if (createdBots == amount)
                {
                    return;
                }
            }
        }
    }

    private void StopCoroutines()
    {
        if (_startBuildingBaseCoroutine != null)
        {
            StopCoroutine(_startBuildingBaseCoroutine);
        }
    }
}
