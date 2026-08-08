using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Kiep : PickupableItem
{
    [SerializeField]
    private ParticleSystem smoke;
    [SerializeField]
    private float hotinReduceSpeed;
    [SerializeField]
    private float kiepDuration;

    public override string UseName => "take a puff";

    private bool isUsing = false;

    protected override void Start()
    {
        base.Start();
        InputAdapter.interact.canceled += OnInteractCanceled;
    }

    private void OnInteractCanceled(InputAction.CallbackContext obj)
    {
        if (!isUsing)
        {
            return;
        }

        StopUsing();
    }

    public override bool CanUse()
    {
        return !isUsing;
    }

    public override void Use()
    {
        isUsing = true;
        smoke.Play();
    }

    public override void OnDrop()
    {
        StopUsing();
        base.OnDrop();
    }

    private void StopUsing()
    {
        isUsing = false;
        smoke.Stop();
    }

    private void Update()
    {
        if (isUsing)
        {
            Hotin.Instance.Value -= hotinReduceSpeed * Time.deltaTime;
            kiepDuration -= Time.deltaTime;
            if (kiepDuration < 0)
            {
                PlayerObjectHolder.Instance.DropObject();
                Destroy(gameObject);
            }
        }
    }
}
