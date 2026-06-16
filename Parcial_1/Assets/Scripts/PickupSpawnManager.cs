using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickupSpawnManager : NetworkBehaviour
{
    public static PickupSpawnManager Instance;

    [SerializeField]
    private GameObject pickupPrefab;

    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private int maxPickups = 4;

    private readonly List<PickupObject> activePickups =
        new();

    private readonly List<int> occupiedPoints =
        new();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        FillMap();
    }

    private void FillMap()
    {
        while (
            activePickups.Count < maxPickups &&
            occupiedPoints.Count < spawnPoints.Length
        )
        {
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        int randomIndex;

        do
        {
            randomIndex =
                Random.Range(
                    0,
                    spawnPoints.Length
                );
        }
        while (
            occupiedPoints.Contains(
                randomIndex
            )
        );

        occupiedPoints.Add(randomIndex);

        GameObject obj =
            Instantiate(
                pickupPrefab,
                spawnPoints[randomIndex].position,
                Quaternion.identity
            );

        NetworkObject netObj =
            obj.GetComponent<NetworkObject>();

        netObj.Spawn();

        PickupObject pickup =
            obj.GetComponent<PickupObject>();

        pickup.SpawnPointIndex =
            randomIndex;

        activePickups.Add(pickup);
    }

    public void RemovePickup(
        PickupObject pickup
    )
    {
        activePickups.Remove(pickup);

        occupiedPoints.Remove(
            pickup.SpawnPointIndex
        );

        FillMap();
    }
}