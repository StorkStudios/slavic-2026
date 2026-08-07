using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObjectHolder : Singleton<PlayerObjectHolder>
{
    [SerializeField]
    private SerializedDictionary<string, Transform> objectHoldLocations;

    public bool IsHoldingObject => currentObject != null;

    private PickupableObject currentObject;

    public void Start()
    {
        InputAdapter.drop.performed += OnDrop;
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

    public void HoldObject(PickupableObject obj)
    {
        if (currentObject != null)
        {
            DropObject();
        }
        obj.transform.parent = transform;
        if (obj.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
        currentObject = obj;
    }

    public PickupableObject DropObject()
    {
        PickupableObject obj = currentObject;
        currentObject = null;
        obj.transform.parent = null;
        if (obj.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
        }
        return obj;
    }
}
