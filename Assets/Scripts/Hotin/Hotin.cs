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

    [HideInInspector]
    public bool canDie = false;

    public event ObservableVariable<float>.ValueChangedDelegate ValueChanged;

    public float Max => hotinRange.Max;

    public float currentGainMultiplier = 1;

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

    public float NormalizedValue => Value / Max;

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
        Value += value * currentGainMultiplier;
    }

    private float ClampValue(float value)
    {
        return Mathf.Clamp(value, hotinRange.Min, hotinRange.Max * (canDie ? 1 : 0.99f));
    }
}
