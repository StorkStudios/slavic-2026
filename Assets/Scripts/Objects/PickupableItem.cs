using DG.Tweening;
using StorkStudios.CoreNest;
using System;
using UnityEngine;

public abstract class PickupableItem : MonoBehaviour, IInteractable, IPickupable
{
    [SerializeField]
    private float pickupDuration;
    [SerializeField]
    private string objectType;

    string IInteractable.ActionName => $"pick up {ObjectType}";
    public string ObjectType => objectType;

    private Collider[] colliders;

    public float HotinGainSpeed => 0;

    public event Action<IPickupable> PickedUp;
    public event Action<IPickupable> Dropped;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public bool CanInteract()
    {
        return !PlayerObjectHolder.Instance.IsHoldingObject;
    }

    public void Interact()
    {
        PlayerObjectHolder objectHolder = PlayerObjectHolder.Instance;
        objectHolder.HoldObject(this);
        Transform holdLocation = objectHolder.GetHoldLocation(objectType);
        transform.parent = holdLocation;
        transform.DOLocalMove(Vector3.zero, pickupDuration);
        transform.DOLocalRotate(Vector3.zero, pickupDuration);
        foreach (Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.HeldItems.GetLayerIndex();
        }
        PickedUp?.Invoke(this);
    }

    public void OnDrop()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.Default.GetLayerIndex();
        }
        Dropped?.Invoke(this);
    }

    public abstract string UseName { get; }
    public abstract bool CanUse();
    public abstract void Use();
}
