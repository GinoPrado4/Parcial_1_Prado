using UnityEngine;


public class SpawnManager : MonoBehaviour


{
    public static SpawnManager Instance;

    public Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;

    }

    public Transform GetSpawnPoint(int index)
    {
        return spawnPoints[index % spawnPoints.Length];
    }



}