using System;
using UnityEngine;

public class Pallet : MonoBehaviour
{
    [SerializeField]
    public string packedMeatObjectType;

    private PickupableObject meatHeldByPlayerOnPallet;

    public void OnPalletTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (other.attachedRigidbody.TryGetComponent(out PickupableObject pickupableObject) &&
            pickupableObject.ObjectType == packedMeatObjectType)
        {
            if (PlayerObjectHolder.Instance.CurrentObject == (IPickupable)pickupableObject)
            {
                meatHeldByPlayerOnPallet = pickupableObject;
                meatHeldByPlayerOnPallet.Dropped += OnItemDroppedOnPallet;
            }
            else
            {
                PackageManager.Instance.AddPackage(pickupableObject);
            }
        }
    }

    public void OnPalletTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (other.attachedRigidbody.TryGetComponent(out PickupableObject pickupableObject) &&
            pickupableObject.ObjectType == packedMeatObjectType)
        {
            if (meatHeldByPlayerOnPallet == pickupableObject)
            {
                meatHeldByPlayerOnPallet.Dropped -= OnItemDroppedOnPallet;
                meatHeldByPlayerOnPallet = null;
            }
            else
            {
                //Removed item put on pallet
                PackageManager.Instance.RemovePackage(pickupableObject);
            }
        }
    }

    private void OnItemDroppedOnPallet(IPickupable _)
    {
        PackageManager.Instance.AddPackage(meatHeldByPlayerOnPallet);
        meatHeldByPlayerOnPallet.Dropped -= OnItemDroppedOnPallet;
        meatHeldByPlayerOnPallet = null;
    }
}
