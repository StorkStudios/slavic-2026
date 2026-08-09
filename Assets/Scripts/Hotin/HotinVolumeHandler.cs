using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class HotinVolumeHandler : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve mappingCurve;

    private AnimationCurve normalizedCurve;
    private Volume volume;

    private void Start()
    {
        normalizedCurve = mappingCurve.GetNormalizedAnimationCurve();
        volume = GetComponent<Volume>();
    }

    private void OnValidate()
    {
        normalizedCurve = mappingCurve.GetNormalizedAnimationCurve();
    }


    private void Update()
    {
        float normalizedHotinValue = Hotin.Instance.Value / Hotin.Instance.Max;
        volume.weight = normalizedCurve.Evaluate(normalizedHotinValue);
    }
}
