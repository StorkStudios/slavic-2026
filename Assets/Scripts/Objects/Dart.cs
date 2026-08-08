using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dart : PickupableItem
{
    [SerializeField]
    private float throwForce;

    public override string UseName => "throw Dart";

    private bool used = false;

    public override bool CanUse()
    {
        return true;
    }

    public override void Use()
    {
        PlayerObjectHolder.Instance.DropObject();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        used = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!used || collision.collider.CompareTag(Tag.Player.GetTagString()))
        {
            return;
        }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        used = false;
    }
}
