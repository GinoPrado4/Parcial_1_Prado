using Unity.Netcode;
using UnityEngine;

public class PickupObject : NetworkBehaviour
{
    private PlayerCarry owner;

    public int SpawnPointIndex { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo tocó el collectible: " + other.name);

        if (!IsServer)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerCarry carry =
            other.GetComponent<PlayerCarry>();

        if (carry == null)
            return;

        if (carry.IsCarryingObject())
            return;

        owner = carry;

        carry.PickObject(this);

        NetworkObject.TrySetParent(
            other.GetComponent<NetworkObject>()
        );

        transform.localPosition =
            new Vector3(0f, 1.5f, 0f);

        GetComponent<Collider>().enabled = false;
    }

    public void Deliver()
    {
        if (!IsServer)
            return;

        PickupSpawnManager.Instance.RemovePickup(this);

        NetworkObject.Despawn(true);
    }
}