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
    [SerializeField]
    private float maxKiepDuration;
    [SerializeField]
    private AudioSource smokeSound;
    [SerializeField]
    private string stopKiepingSound;

    public override string UseName => "take a puff";

    private bool isUsing = false;

    protected override void Start()
    {
        base.Start();
        InputAdapter.interact.canceled += OnInteractCanceled;
    }

    private void OnDestroy()
    {
        InputAdapter.interact.canceled -= OnInteractCanceled;
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
        smokeSound.Play();
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
        smokeSound.Stop();
        if (!string.IsNullOrEmpty(stopKiepingSound))
        {
            CommonSoundManager.Instance.PlaySound(stopKiepingSound);
        }
    }

    private void Update()
    {
        if (isUsing)
        {
            Hotin.Instance.Value -= hotinReduceSpeed * Time.deltaTime;
            kiepDuration -= Time.deltaTime;
            if (kiepDuration < 0)
            {
                SetZScale(0);
                PlayerObjectHolder.Instance.DropObject();
                Destroy(gameObject);
            }
            else
            {
                SetZScale(kiepDuration / maxKiepDuration);
            }
        }
    }

    private void SetZScale(float z)
    {
        Vector3 scale = transform.localScale;
        scale.z = z;
        transform.localScale = scale;
    }
}
