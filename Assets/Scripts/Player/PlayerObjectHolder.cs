using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectHolder : Singleton<PlayerObjectHolder>
{
    [SerializeField]
    private SerializedDictionary<string, Transform> objectHoldLocations;

    public string DropActionName => $"drop {currentObject.ObjectType}";

    public bool IsHoldingObject => currentObject != null;

    private IPickupable currentObject;
    public IPickupable CurrentObject => currentObject;

    public void Start()
    {
        InputAdapter.drop.performed += OnDrop;
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
        if (!IsHoldingObject)
        {
            return;
        }

        DropObject();
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
        }
        return obj;
    }
}
