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
        InputAdapter.checkTime.performed += OnTimeCheckInput;
    }

    public bool CanCheckTime()
    {
        return PlayerObjectHolder.Instance.CurrentObject == null &&
            Minigame.CurrentMinigame == null &&
            UIPicturesController.Instance.CurrentPicture == null &&
            PlayerController.Instance.Active;
    }

    public void CheckTime()
    {
        if (!CanCheckTime())
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

    private void OnTimeCheckInput(InputAction.CallbackContext context)
    {
        CheckTime();
    }
}
