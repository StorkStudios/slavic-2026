using UnityEngine;

public class BloodController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve remapNormalizedHotinToScale;

    private void Start()
    {
        Hotin.Instance.ValueChanged += OnHotinChanged;
        SetYScale(0);
    }

    private void OnDestroy()
    {
        Hotin.Instance.ValueChanged -= OnHotinChanged;
    }

    private void OnHotinChanged(float oldValue, float newValue)
    {
        float normalizedValue = newValue / Hotin.Instance.Max;
        normalizedValue = remapNormalizedHotinToScale.Evaluate(normalizedValue);
        SetYScale(normalizedValue);
    }

    private void SetYScale(float y)
    {
        Vector3 scale = transform.localScale;
        scale.y = y;
        transform.localScale = scale;
    }
}
