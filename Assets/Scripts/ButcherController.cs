using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ButcherController : MonoBehaviour
{
    [SerializeField]
    private float playerKillCameraRotationVeclocity;

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
        }
    }

    private void KillPlayerAndDestroyButcher()
    {
        Hotin.Instance.canDie = true;
        Hotin.Instance.Add(Hotin.Instance.Max);
        Hotin.Instance.canDie = false;

        Destroy(gameObject);
    }

    private void StartPlayerKillSequence()
    {
        if (Minigame.CurrentMinigame != null)
        {
            Minigame.CurrentMinigame.EndMinigame(false);
        }
        if (PlayerObjectHolder.Instance.CurrentObject != null)
        {
            PlayerObjectHolder.Instance.DropObject();
        }
        PlayerController.Instance.Active = false;

        Transform cameraTransfrom = Camera.main.transform;
        Vector3 fromPlayerToButcher = transform.position - cameraTransfrom.position;
        float angle = Vector3.Angle(fromPlayerToButcher, cameraTransfrom.forward);
        cameraTransfrom.DOLookAt(fromPlayerToButcher, angle / playerKillCameraRotationVeclocity, AxisConstraint.Z, Vector3.up)
            .SetEase(Ease.OutQuad)
            .OnComplete(KillPlayerAndDestroyButcher);
    }
}
