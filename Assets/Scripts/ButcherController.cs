using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ButcherController : MonoBehaviour
{
    [SerializeField]
    private Transform playerLookTarget;
    public Transform PlayerLookTarget => playerLookTarget;

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
            if (path.corners.Length < 2 || agent.remainingDistance < 0.5f || (PlayerController.Instance.transform.position - path.corners.Last()).sqrMagnitude < 0.25f)
            {
                moving = false;
            }
        }
    }

    public void OnPlayerCaught(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            StartPlayerKillSequence();
            agent.acceleration = 99999f;
            agent.isStopped = true;
        }
    }

    private void StartPlayerKillSequence()
    {
        PlayerButcherDier.Instance.PlayKillSequence(this);
    }
}
