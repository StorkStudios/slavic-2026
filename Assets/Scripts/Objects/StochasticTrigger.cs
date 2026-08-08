using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.Events;

public class StochasticTrigger : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float setSuccessChance;
    [SerializeField]
    private Trigger trigger = new();

    public event UnityAction OnTrigger
    {
        add => trigger.AddListener(value);
        remove => trigger.RemoveListener(value);
    }

    public void Set()
    {
        if (setSuccessChance == 0 || Random.value > setSuccessChance)
        {
            return;
        }

        trigger.Set(name);
    }
}
