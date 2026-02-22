using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Zone
{
    public float progressBetweenObstacles;
    public List<GameObject> obstacles;
}

public class ObstacleSpawner : MonoBehaviour
{
    public List<Zone> zones;
    public float spawnXRange;
    public float spawnAheadDistance;

    private PlayerMovement playerMovement;
    private ProgressTracker progressTracker;

    private int nextZoneProgresssObstacle;
    private float nextProgresssObstacle;

    void Start()
    {
        progressTracker = FindAnyObjectByType<ProgressTracker>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        SetNextObstacleSpawn();
    }

    void Update()
    {
        if(progressTracker.zone >= nextZoneProgresssObstacle && progressTracker.zoneProgress >= nextProgresssObstacle && progressTracker.zone < 4)
        {
            SpawnObstacle();
            SetNextObstacleSpawn();
        }
    }

    private void SetNextObstacleSpawn()
    {
        if(nextProgresssObstacle + zones[nextZoneProgresssObstacle].progressBetweenObstacles < 1)
        {
            nextProgresssObstacle += zones[nextZoneProgresssObstacle].progressBetweenObstacles;
        }
        else
        {
            nextProgresssObstacle = zones[nextZoneProgresssObstacle].progressBetweenObstacles;
            nextZoneProgresssObstacle += 1;
        }
    }

    private void SpawnObstacle()
    {
        GameObject obstacle = zones[nextZoneProgresssObstacle].obstacles[UnityEngine.Random.Range(0, zones[nextZoneProgresssObstacle].obstacles.Count)];

        Instantiate(obstacle, (Vector2)playerMovement.transform.position + Vector2.up * spawnAheadDistance + Vector2.right * UnityEngine.Random.Range(-spawnXRange, spawnXRange), Quaternion.identity);
    }
}
