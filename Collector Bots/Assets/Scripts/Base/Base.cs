using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Base : MonoBehaviour
{
    public int ResourcesAmount { get; private set; }
    public Flag CurrentFlag { get; private set; }
    public bool IsBuildingBase { get; private set; }

    [SerializeField] private Transform _collectorsPlace;

    [SerializeField] private Text _resourcesAmountText;

    [SerializeField] private MeshRenderer _meshRenderer;

    private LayerMask _resourcesLayer;
    private int _startCollectorsAmount;
    private int _newBasePrice;
    private Collector _collectorTemplate;
    private int _collectorPrice;
    private Transform[] _collectorsPlaces;
    private Collector[] _collectors;
    private Material _originalMaterial;
    private Collector _freeCollector;
    private BaseController _baseController;
    private int _baseBuilderIndex;
    private int _collectorsAmount;

    private void Awake()
    {
        IsBuildingBase = false;
        _baseBuilderIndex = -1;
        _collectorsAmount = 0;
    }

    private void Update()
    {
        CheckEvents();
    }

    private void OnValidate()
    {
        ValidateBotsStartAmount();
    }

    private void OnDestroy()
    {
        StopCoroutine(StartBuildingBase());
    }

    private void OnEnable()
    {
        StopCoroutine(StartBuildingBase());
    }

    public void SetData(Collector collectorTemplate, int collectorPrice, LayerMask resourcesLayer, int startCollectorsAmount, int newBasePrice, BaseController baseController)
    {
        _originalMaterial = _meshRenderer.material;
        ResourcesAmount = 0;
        _collectorsPlaces = new Transform[_collectorsPlace.childCount];
        _collectors = new Collector[_collectorsPlace.childCount];

        _collectorTemplate = collectorTemplate;
        _collectorPrice = collectorPrice;
        _resourcesLayer = resourcesLayer;
        _startCollectorsAmount = startCollectorsAmount;
        _newBasePrice = newBasePrice;
        _baseController = baseController;

        for (int i = 0; i < _collectorsPlaces.Length; i++)
        {
            _collectorsPlaces[i] = _collectorsPlace.GetChild(i);
        }

        CreateCollectors(_startCollectorsAmount);
    }

    public bool TrySendCollector()
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

    private void CheckEvents()
    {
        _resourcesAmountText.text = ResourcesAmount.ToString();

        if (CurrentFlag != null && IsBuildingBase == false)
        {
            if (ResourcesAmount >= _newBasePrice)
            {
                IsBuildingBase = true;
                ResourcesAmount -= _newBasePrice;
                StartCoroutine(StartBuildingBase());
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

    private void ValidateBotsStartAmount()
    {
        if (_collectorsPlace == null)
        {
            _startCollectorsAmount = 0;
        }
        else if (_startCollectorsAmount > _collectorsPlace.childCount)
        {
            _startCollectorsAmount = _collectorsPlace.childCount;
        }
        else if (_startCollectorsAmount < 0)
        {
            _startCollectorsAmount = 0;
        }
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
}
