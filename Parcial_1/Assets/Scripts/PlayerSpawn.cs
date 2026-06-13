using Unity.Netcode;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {

        Debug.Log("SPAWN EJECUTADO");

        if (!IsServer) return;

        Transform spawnPoint =
            SpawnManager.Instance.GetSpawnPoint((int)OwnerClientId);

        transform.SetPositionAndRotation(
            spawnPoint.position,
            spawnPoint.rotation
        );


    }
}
