using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _minResourceSpawnDelay;
    [SerializeField] private float _maxResourceSpawnDelay;
    [SerializeField] private Resource _resourceTemplate;

    private Transform[] _spawnPoints;

    private void Start()
    {
        _spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(Spawn());
    }

    private void OnDestroy()
    {
        StopCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        bool isWorking = enabled;

        while (isWorking)
        {
            float seconds = Random.Range(_minResourceSpawnDelay, _maxResourceSpawnDelay);
            yield return new WaitForSeconds(seconds);

            int pointIndex = Random.Range(0, _spawnPoints.Length);

            Instantiate(_resourceTemplate, _spawnPoints[pointIndex].position, Quaternion.identity);
        }
    }
}
