using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _maxSpawnDelay;
    [SerializeField] private Resource _resourceTemplate;

    private Transform[] _spawnPoints;
    private float _waitTime;

    private void Start()
    {
        _spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        bool isWorking = enabled;

        while (isWorking)
        {
            _waitTime = Random.Range(_minSpawnDelay, _maxSpawnDelay);
            yield return new WaitForSeconds(_waitTime);

            int pointIndex = Random.Range(0, _spawnPoints.Length);

            Instantiate(_resourceTemplate, _spawnPoints[pointIndex].position, Quaternion.identity);
        }
    }
}
