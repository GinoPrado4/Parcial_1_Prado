using Unity.Netcode;
using UnityEngine;

public class SpawnPlayers : NetworkBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            int index = (int)clientId % spawnPoints.Length;

            GameObject player = Instantiate(playerPrefab,
                spawnPoints[index].position,
                spawnPoints[index].rotation);

            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}
