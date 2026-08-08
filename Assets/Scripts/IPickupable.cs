using UnityEngine;

public interface IPickupable
{
    public string ObjectType { get; }
    public float HotinGainSpeed { get; }
    public Transform transform { get; }

    public void OnDrop();
}
