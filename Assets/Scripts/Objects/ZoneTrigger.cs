using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private Trigger enterTrigger;
    [SerializeField]
    private Trigger stayTrigger;
    [SerializeField]
    private Trigger leaveTrigger;

    private bool wasInside = false;
    private bool isInside = false;

    private void Awake()
    {
        if(!GetComponent<Collider>().isTrigger)
        {
            Debug.LogError($"Collider for {nameof(ZoneTrigger)} is not trigger in {gameObject.name}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tag.Player.GetTagString()))
        {
            isInside = true;
        }
    }

    private void FixedUpdate()
    {
        if (isInside != wasInside)
        {
            if (isInside)
            {
                enterTrigger.Set($"{gameObject.name}.Enter");
            }
            else
            {
                leaveTrigger.Set($"{gameObject.name}.Exit");
            }
        }

        if (isInside)
        {
            stayTrigger.Set($"{gameObject.name}.Stay");
        }

        wasInside = isInside;
        isInside = false;
    }
}
