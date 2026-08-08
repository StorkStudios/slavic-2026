using UnityEngine;

public interface IPickupable
{
    public string ObjectType { get; }
    public float HotinGainSpeed { get; }
    public Transform transform { get; }
    public string DropSound { get; }

    public event System.Action<IPickupable> PickedUp;
    public event System.Action<IPickupable> Dropped;

    public void OnDrop();
}
