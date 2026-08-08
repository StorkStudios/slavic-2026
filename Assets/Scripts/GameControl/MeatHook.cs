using StorkStudios.CoreNest;
using UnityEngine;

public class MeatHook : MonoBehaviour
{
    [SerializeField]
    private Trigger onUnlock;
    [SerializeField]
    private Trigger onPickup;
    [SerializeField]
    private PickupableObject meat;

    public bool IsLocked { get; private set; }

    private void Start()
    {
        IsLocked = true;
        meat.locked = true;
    }

    public void Unlock(System.Action onPickup)
    {
        IsLocked = false;
        meat.locked = false;
        onUnlock.Set("Meat unlock");

        void OnPickedUp(IPickupable obj)
        {
            this.onPickup.Set("Meat pickup");
            onPickup?.Invoke();
            obj.PickedUp -= OnPickedUp;
        }

        meat.PickedUp += OnPickedUp;
    }
}
