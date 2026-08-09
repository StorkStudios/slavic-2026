using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dart : PickupableItem
{
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float boardHitHodinReduceValue;
    [SerializeField]
    private string throwSound;
    [SerializeField]
    private AudioSource hitSound;
    [SerializeField]
    private AudioSource hitTargetSound;

    public override string UseName => "throw Dart";

    private bool used = false;
    private Rigidbody rigidbody;

    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody>();
    }

    public override bool CanUse()
    {
        return true;
    }

    public override void Use()
    {
        PlayerObjectHolder.Instance.DropObject();
        rigidbody.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        used = true;

        if (!string.IsNullOrEmpty(throwSound))
        {
            CommonSoundManager.Instance.PlaySound(throwSound);
        }
    }

    private void Update()
    {
        if (used && rigidbody.linearVelocity.magnitude > 0.01f)
        {
            transform.forward = rigidbody.linearVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!used || collision.collider.CompareTag(Tag.Player.GetTagString()))
        {
            return;
        }

        rigidbody.isKinematic = true;
        used = false;

        if (isInDartBoard)
        {
            Hotin.Instance.Value -= boardHitHodinReduceValue;
            hitTargetSound.Play();
        }
        else
        {
            hitSound.Play();
        }
    }

    private bool isInDartBoard = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Dartboard.GetTagString()))
        {
            isInDartBoard = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.Dartboard.GetTagString()))
        {
            isInDartBoard = false;
        }
    }
}
