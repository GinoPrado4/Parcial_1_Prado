using Unity.Netcode;
using UnityEngine;

public class PlayerCarry : NetworkBehaviour
{
    private PickupObject carriedObject;

    public bool IsCarryingObject()
    {
        return carriedObject != null;
    }

    public void PickObject(PickupObject pickup)
    {
        carriedObject = pickup;
    }

    public PickupObject GetCarriedObject()
    {
        return carriedObject;
    }

    public void ClearObject()
    {
        carriedObject = null;
    }
}