using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(fileName = "Hotin", menuName = "ScriptableObjects/Hotin")]
public class Hotin : ScriptableObjectSingleton<Hotin>
{
    [SerializeField]
    private RangeBoundariesFloat hotinRange;
    [ReadOnly]
    [SerializeField]
    private float hotinValue;

    public event ObservableVariable<float>.ValueChangedDelegate ValueChanged;

    public float Max => hotinRange.Max;

    public float Value
    {
        get => hotinValue;
        set
        {
            float newValue = ClampValue(value);
            float oldValue = hotinValue;
            if (newValue != oldValue)
            {
                hotinValue = newValue;
                ValueChanged?.Invoke(oldValue, newValue);
            }
        }
    }

    public void ResetNoUpdate()
    {
        hotinValue = hotinRange.Min;
    }

    public void AddDeltaTimeScaled(float scale)
    {
        float dt = Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
        Add(dt * scale);
    }

    public void Add(float value)
    {
        Value += value;
    }

    private float ClampValue(float value)
    {
        return Mathf.Clamp(value, hotinRange.Min, hotinRange.Max);
    }
}
