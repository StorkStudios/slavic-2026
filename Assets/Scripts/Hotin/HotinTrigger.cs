using StorkStudios.CoreNest;
using UnityEngine;

public class HotinTrigger : MonoBehaviour
{
    [SerializeField]
    private RangeBoundariesFloat hotinRange;

    [SerializeField]
    private Trigger rangeEnter;
    [SerializeField]
    private Trigger rangeStay;
    [SerializeField]
    private Trigger rangeExit;

    private bool isInRange = false;

    private void Start()
    {
        Hotin.Instance.ValueChanged += OnHotinChange;
    }

    private void OnHotinChange(float oldValue, float newValue)
    {
        isInRange = hotinRange.Contains(newValue, BoundariesType.IncludeMin);
        if (isInRange)
        {
            rangeEnter.Set($"{gameObject.name}.HotinEnter");
        }
        else
        {
            rangeExit.Set($"{gameObject.name}.HotinExit");
        }
    }

    private void OnDestroy()
    {
        Hotin.Instance.ValueChanged -= OnHotinChange;
    }


    private void Update()
    {
        if (isInRange)
        {
            rangeStay.Set($"{gameObject.name}.HotinStay");
        }
    }
}
