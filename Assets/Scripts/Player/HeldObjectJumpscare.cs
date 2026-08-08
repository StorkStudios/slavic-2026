using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;

public class HeldObjectJumpscare : MonoBehaviour
{
    [Header("Check")]
    [SerializeField]
    private float minHotinValue;
    [SerializeField]
    private float checkCooldown;
    [Range(0, 1)]
    [SerializeField]
    private float jumpscareChance;

    [Header("Jumpscare")]
    [SerializeField]
    private float jumpscareDuration;
    [SerializeField]
    private AudioSource jumpscareSound;
    [SerializeField]
    private float shakeStrength;
    [SerializeField]
    private int shakeVibrato;
    [SerializeField]
    private float shakeRandomness;

    private float nextCheckTimestamp;

    private void Update()
    {
        if (Hotin.Instance.Value < minHotinValue ||
            PlayerObjectHolder.Instance.CurrentObject is not PickupableObject obj)
        {
            nextCheckTimestamp = Time.time + checkCooldown;
            return;
        }

        if (Time.time > nextCheckTimestamp)
        {
            nextCheckTimestamp = Time.time + checkCooldown;

            if (jumpscareChance < 0 || Random.value > jumpscareChance)
            {
                return;
            }

            Jumpscare();
        }
    }

    private void Jumpscare()
    {
        PickupableObject obj = (PickupableObject) PlayerObjectHolder.Instance.DropObject();
        if (obj.transform.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
        obj.locked = true;
        Vector3 playerForward = PlayerController.Instance.transform.forward;
        jumpscareSound.Play();
        obj.transform.DOShakeRotation(jumpscareDuration, shakeStrength, shakeVibrato, shakeRandomness, false);
        obj.transform.DOBlendableMoveBy(Vector3.up + playerForward, jumpscareDuration);
        this.CallDelayed(jumpscareDuration, () =>
        {
            jumpscareSound.Stop();
            if (obj.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
            }
            obj.locked = false;
        });
    }
}
