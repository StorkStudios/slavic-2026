using StorkStudios.CoreNest;
using UnityEngine;

public class PlayerObjectHolder : Singleton<PlayerObjectHolder>
{
    [SerializeField]
    private SerializedDictionary<string, Transform> objectHoldLocations;

    public bool IsHoldingObject => currentObject != null;

    private GameObject currentObject;

    public Transform GetHoldLocation(string objectType)
    {
        if (!objectHoldLocations.TryGetValue(objectType, out Transform holdLocation))
        {
            holdLocation = null;
        }
        return holdLocation;
    }

    public void HoldObject(GameObject obj)
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

    public GameObject DropObject()
    {
        GameObject obj = currentObject;
        currentObject = null;
        obj.transform.parent = null;
        if (obj.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
        }
        return obj;
    }
}
