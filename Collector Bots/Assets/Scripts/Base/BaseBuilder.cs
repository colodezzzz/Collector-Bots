using System.Collections;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    public bool IsBuildingBase { get; private set; }

    private CollectorsCreater _collectorsCreater;
    private Base _base;
    private BaseCreator _baseCreater;

    private Coroutine _startBuildingBaseCoroutine;
    private int _baseBuilderIndex;

    private void OnDisable()
    {
        StopCoroutines();
    }

    public void SetFields(CollectorsCreater collectorsCreater, Base parentBase, BaseCreator baseCreater)
    {
        _collectorsCreater = collectorsCreater;
        _base = parentBase;
        _baseCreater = baseCreater;
    }

    public void BuildBase()
    {
        _collectorsCreater.DeleteCollector(_baseBuilderIndex);
        _baseBuilderIndex = -1;
        IsBuildingBase = false;
        _baseCreater.CreateBase(_base.CurrentFlag.transform.position);
        _base.UnsetFlag();
    }

    public void StartBuildBase()
    {
        IsBuildingBase = true;
        _startBuildingBaseCoroutine = StartCoroutine(BuildingBase());
    }

    private IEnumerator BuildingBase()
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

    private bool TryStartBuildBase()
    {
        for (int i = 0; i < _collectorsCreater.Collectors.Length; i++)
        {
            if (_collectorsCreater.Collectors[i] != null && _collectorsCreater.Collectors[i].Target == null)
            {
                _baseBuilderIndex = i;
                _collectorsCreater.Collectors[i].StartBuildBase(_base.CurrentFlag.transform);
                return true;
            }
        }

        return false;
    }

    private void StopCoroutines()
    {
        if (_startBuildingBaseCoroutine != null)
        {
            StopCoroutine(_startBuildingBaseCoroutine);
        }
    }
}
