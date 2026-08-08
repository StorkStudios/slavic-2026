using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skrzynka : PickupableItem, IInteractable
{
    [SerializeField]
    private Transform containerBox;

    private List<Rigidbody> items;

    public override string UseName => "";

    protected override void Start()
    {
        base.Start();
        PickedUp += OnPickedUp;
        Dropped += OnDropped;
    }

    private void OnPickedUp(IPickupable obj)
    {
        Collider[] colliders = Physics.OverlapBox(containerBox.position, containerBox.lossyScale / 2, containerBox.rotation);
        items = colliders.Where(collider => collider.attachedRigidbody != null && !collider.attachedRigidbody.isKinematic)
            .Select(collider => collider.attachedRigidbody).ToList();

        foreach (Rigidbody rigidbody in items)
        {
            rigidbody.isKinematic = true;
            rigidbody.transform.parent = transform;
        }
    }

    private void OnDropped(IPickupable obj)
    {
        foreach (Rigidbody rigidbody in items)
        {
            rigidbody.isKinematic = false;
            rigidbody.transform.parent = null;
        }
        items.Clear();
    }

    public override bool CanUse()
    {
        return false;
    }

    public override void Use()
    {

    }
}
