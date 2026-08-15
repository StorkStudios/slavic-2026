using DG.Tweening;
using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IInteractable, IPickupable
{
    [SerializeField]
    private float pickupHalfDuration;
    [SerializeField]
    private string objectType;
    [SerializeField]
    private float hotinGainSpeed;
    [SerializeField]
    private string interactSound;
    [SerializeField]
    private string dropSound;

    public string ObjectType => objectType;
    string IInteractable.ActionName => "pick up " + ObjectType;
    public string InteractSound => interactSound;
    public string DropSound => dropSound;
    private Collider[] colliders;

    public event System.Action<IPickupable> PickedUp;
    public event System.Action<IPickupable> Dropped;

    [HideInInspector]
    public bool locked = false;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public float HotinGainSpeed => hotinGainSpeed;

    public bool CanInteract()
    {
        return !locked && !PlayerObjectHolder.Instance.IsHoldingObject;
    }

    public void Interact()
    {
        PlayerObjectHolder.Instance.LookAt(transform);
        PlayerController.Instance.Active = false;
        this.CallDelayed(pickupHalfDuration, OnPickupBack);
    }

    public void OnDrop()
    {
        foreach(Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.Default.GetLayerIndex();
        }
        Dropped?.Invoke(this);
    }

    private void OnPickupBack()
    {
        PlayerObjectHolder objectHolder = PlayerObjectHolder.Instance;
        objectHolder.LookAt(null);
        objectHolder.HoldObject(this);
        Transform holdLocation = objectHolder.GetHoldLocation(objectType);
        transform.DOMove(holdLocation.position, pickupHalfDuration);
        transform.DORotate(holdLocation.eulerAngles, pickupHalfDuration);
        transform.DOScale(holdLocation.lossyScale, pickupHalfDuration);
        this.CallDelayed(pickupHalfDuration, () => PlayerController.Instance.Active = true);
        foreach(Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.HeldItems.GetLayerIndex();
        }
        PickedUp?.Invoke(this);
    }
}
