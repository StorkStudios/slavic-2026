using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ButcherController : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private bool moving;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        moving = false;
        path = new();
    }

    private void Update()
    {
        if (!moving)
        {
            if (agent.CalculatePath(PlayerController.Instance.transform.position, path))
            {
                agent.SetPath(path);
                moving = true;
            }
        }
        else
        {
            if (agent.remainingDistance < 0.5f || (PlayerController.Instance.transform.position - path.corners.Last()).sqrMagnitude < 0.25f)
            {
                moving = false;
            }
        }
    }

    public void OnPlayerCaught(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            Hotin.Instance.Value = 999999f;
            Destroy(gameObject);
        }
    }
}
