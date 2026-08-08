using System;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWatchWatcher : Singleton<PlayerWatchWatcher>
{
    [SerializeField]
    private Animator animator;

    private bool watching;
    public bool Watching => watching;

    private void Start()
    {
        InputAdapter.checkTime.performed += OnTimeCheck;
    }

    private void OnTimeCheck(InputAction.CallbackContext context)
    {
        if (PlayerObjectHolder.Instance.CurrentObject != null)
        {
            return;
        }

        watching = !watching;
        if (watching)
        {
            animator.CrossFade("CheckWatch", 0.5f);
        }
        else
        {
            animator.CrossFade(PlayerController.Instance.MovedLastFrame ? "Run" : "Idle", 0.5f);
        }
    }
}
