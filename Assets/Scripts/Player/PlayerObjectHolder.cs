using DG.Tweening;
using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectHolder : Singleton<PlayerObjectHolder>
{
    [SerializeField]
    private SerializedDictionary<string, Transform> objectHoldLocations;
    [SerializeField]
    private SerializedDictionary<string, Transform> objectDropLocations;

    [SerializeField]
    private SerializedDictionary<string, string> objectHoldAnimations;

    [SerializeField]
    private SerializedDictionary<string, float> objectThrowForce;

    [SerializeField]
    private PlayerAnimationController animator;
    [SerializeField]
    private string defaultObjectHoldAnimation;
    [SerializeField]
    private CinemachineCamera pickupCamera;
    [SerializeField]
    private float dropAnimationDuration;

    public string DropActionName => $"Drop {currentObject.ObjectType}";

    public bool IsHoldingObject => currentObject != null;

    private IPickupable currentObject;
    public IPickupable CurrentObject => currentObject;

    public void Start()
    {
        InputAdapter.drop.performed += OnDrop;
    }

    protected override void OnDestroy()
    {
        InputAdapter.drop.performed -= OnDrop;
        base.OnDestroy();
    }

    public void Update()
    {
        if (IsHoldingObject && CurrentObject.HotinGainSpeed > 0)
        {
            Hotin.Instance.AddDeltaTimeScaled(CurrentObject.HotinGainSpeed);
        }
    }

    private void OnDrop(InputAction.CallbackContext obj)
    {
        if (!IsHoldingObject || UIPicturesController.Instance.CurrentPicture != null)
        {
            return;
        }
        if (!string.IsNullOrEmpty(currentObject.DropSound))
        {
            CommonSoundManager.Instance.PlaySound(currentObject.DropSound);
        }
        if (objectDropLocations.TryGetValue(CurrentObject.ObjectType, out Transform dropTransform))
        {
            CurrentObject.transform.SetParent(dropTransform, true);
            CurrentObject.transform.DOLocalRotate(Vector3.zero, dropAnimationDuration);
            CurrentObject.transform.DOLocalMove(Vector3.zero, dropAnimationDuration).OnComplete(() => DropObject());
        }
        else
        {
            DropObject();
        }
    }

    public Transform GetHoldLocation(string objectType)
    {
        if (!objectHoldLocations.TryGetValue(objectType, out Transform holdLocation))
        {
            holdLocation = null;
        }
        return holdLocation;
    }

    public void HoldObject(IPickupable obj)
    {
        if (currentObject != null)
        {
            DropObject();
        }
        obj.transform.parent = transform;
        if (obj.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
        currentObject = obj;
        
        if (objectHoldAnimations.ContainsKey(obj.ObjectType))
        {
            animator.CrossFade(objectHoldAnimations[obj.ObjectType], 0.5f);
        }
        else
        {
            animator.CrossFade(defaultObjectHoldAnimation, 0.5f);
        }
    }

    public IPickupable DropObject()
    {
        IPickupable obj = currentObject;
        currentObject = null;
        obj.transform.parent = null;
        obj.OnDrop();
        if (obj.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
            if (objectThrowForce.TryGetValue(obj.ObjectType, out float force))
            {
                Vector3 forceVector = PlayerController.Instance.transform.forward * force;
                rigidbody.AddForce(forceVector, ForceMode.VelocityChange);
            }
        }
        animator.CrossFade(PlayerController.Instance.MovedLastFrame ? "Run" : "Idle", 0.5f);
        return obj;
    }

    public void LookAt(Transform transform)
    {
        pickupCamera.Target.LookAtTarget = transform;
        pickupCamera.enabled = transform != null;
    }
}
