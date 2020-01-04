using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _spawnEnemySpeed = 2.0f;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] _powerUpPrefab;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine(_spawnEnemySpeed));
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    // spawn game object every 5 sec using coroutine
    // Create  coroutine of type IEnumerator -- Yield event
    private IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        while (! _stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-7f, 7f), 7f, 0f);
            GameObject spawnEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            // put the new object inside spawnManager child enemyContainer
            spawnEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (! _stopSpawning)
        {
            float waitTime = Random.Range(4.0f, 8.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-7f, 7f), 7f, 0f);

            int randomPowerUp = Random.Range(0, 2);
            GameObject spawnPowerup = Instantiate(_powerUpPrefab[randomPowerUp], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
