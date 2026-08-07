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

    private void OnPickupBack()
    {
        animationCameraTarget.enabled = false;
        PlayerObjectHolder objectHolder = PlayerObjectHolder.Instance;
        objectHolder.HoldObject(gameObject);
        Transform holdLocation = objectHolder.GetHoldLocation(objectType);
        transform.DOMove(holdLocation.position, pickupHalfDuration);
        transform.DORotate(holdLocation.eulerAngles, pickupHalfDuration);
        this.CallDelayed(pickupHalfDuration, () => PlayerController.Instance.active = true);
    }
}
