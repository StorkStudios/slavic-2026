using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CinemachineCamera animationCameraTarget;

    public void Interact(GameObject playerObject)
    {
        animationCameraTarget.enabled = true;
        this.CallDelayed(2, () => animationCameraTarget.enabled = false);
    }
}
