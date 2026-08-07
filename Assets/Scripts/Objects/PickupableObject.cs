using DG.Tweening;
using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CinemachineCamera animationCameraTarget;
    [SerializeField]
    private float pickupHalfDuration;
    [SerializeField]
    private string objectType;
    [SerializeField]
    private float hotinGainSpeed;

    public string ObjectType => objectType;
    string IInteractable.ActionName => "pick up " + ObjectType;
    private Collider[] colliders;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public float HotinGainSpeed => hotinGainSpeed;

    public bool CanInteract()
    {
        return !PlayerObjectHolder.Instance.IsHoldingObject;
    }

    public void Interact()
    {
        animationCameraTarget.enabled = true;
        PlayerController.Instance.active = false;
        this.CallDelayed(pickupHalfDuration, OnPickupBack);
    }

    public void OnDrop()
    {
        foreach(Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.Default.GetLayerIndex();
        }
    }

    private void OnPickupBack()
    {
        animationCameraTarget.enabled = false;
        PlayerObjectHolder objectHolder = PlayerObjectHolder.Instance;
        objectHolder.HoldObject(this);
        Transform holdLocation = objectHolder.GetHoldLocation(objectType);
        transform.DOMove(holdLocation.position, pickupHalfDuration);
        transform.DORotate(holdLocation.eulerAngles, pickupHalfDuration);
        transform.DOScale(holdLocation.lossyScale, pickupHalfDuration);
        this.CallDelayed(pickupHalfDuration, () => PlayerController.Instance.active = true);
        foreach(Collider collider in colliders)
        {
            collider.gameObject.layer = Layer.HeldItems.GetLayerIndex();
        }
    }
}
