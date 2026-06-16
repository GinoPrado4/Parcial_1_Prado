using Unity.Netcode;
using UnityEngine;

public class DeliveryZone : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerCarry carry =
            other.GetComponent<PlayerCarry>();

        if (carry == null)
            return;

        if (!carry.IsCarryingObject())
            return;

        PickupObject pickup =
            carry.GetCarriedObject();

        pickup.Deliver();

        carry.ClearObject();

        ulong playerId =
            other.GetComponent<NetworkObject>()
                 .OwnerClientId;

        GameManager.Instance.AddPoint(playerId);

        
    }
}