using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles spawning of Enemies and HealthBonus
/// </summary>
public class SpawnManager : MonoBehaviour
{
    public static float enemyRepeatRate;
    public static float healthRepeatRate;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject healthBonusPrefab;
    private GameObject healthBonus;
    private float xSpawnRange = 20.0f;
    private float zSpawnPosBot = 0f;
    private float zSpawnPosTop = 22.5f;

    void Awake()
    {
        healthBonus = Instantiate(healthBonusPrefab);
        healthBonus.SetActive(false);
    }

    void OnEnable()
    {
        InvokeRepeating("SpawnRandomEnemy", 2.0f, enemyRepeatRate);
        InvokeRepeating("SpawnHealthBonus", 7.0f, healthRepeatRate);
    }

    void OnDisable()
    {
        if (healthBonus != null)
        {
            healthBonus.SetActive(false);
        }
        CancelInvoke("SpawnRandomEnemy");
        CancelInvoke("SpawnHealthBonus");
    }

    private void SpawnRandomEnemy()
    {
        //Spawn lines are used to spawn enemies outside game view
        float zSpawnLineTop = 30.0f;
        float zSpawnLineBot = -7.5f;
        float xSpawnLine = 30.0f;
        int animalIndex = Random.Range(0, enemyPrefabs.Length);
        int movingDirection = Random.Range(0, 4);

        switch (movingDirection)
        {
            case 0: //From top
                Vector3 spawnPosTop = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), 0, zSpawnLineTop);
                Instantiate(enemyPrefabs[animalIndex], spawnPosTop, enemyPrefabs[animalIndex].transform.rotation);
                break;
            case 1: //From bottom
                Vector3 spawnPosBot = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), 0, zSpawnLineBot);
                Instantiate(enemyPrefabs[animalIndex], spawnPosBot, Quaternion.Euler(0, 0, 0));
                break;
            case 2: //From left
                Vector3 spawnPosLeft = new Vector3(-xSpawnLine, 0, Random.Range(zSpawnPosBot, zSpawnPosTop));
                Instantiate(enemyPrefabs[animalIndex], spawnPosLeft, Quaternion.Euler(0, 90, 0));
                break;
            case 3: //From right
                Vector3 spawnPosRight = new Vector3(xSpawnLine, 0, Random.Range(zSpawnPosBot, zSpawnPosTop));
                Instantiate(enemyPrefabs[animalIndex], spawnPosRight, Quaternion.Euler(0, -90, 0));
                break;
        }
    }

    private void SpawnHealthBonus()
    {
        float zSpawnPos = Random.Range(zSpawnPosBot, zSpawnPosTop);
        float xSpawnPos = Random.Range(-xSpawnRange, xSpawnRange);
        float ySpawnPos = healthBonusPrefab.transform.position.y;
        Quaternion spawnRot = healthBonusPrefab.transform.rotation;

        healthBonus.transform.position = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        healthBonus.transform.rotation = spawnRot;
        if (!healthBonus.activeInHierarchy)
        {
            healthBonus.SetActive(true);
        }
    }
}
