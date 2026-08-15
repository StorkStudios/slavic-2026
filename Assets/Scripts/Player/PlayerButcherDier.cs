using System;
using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerButcherDier : Singleton<PlayerButcherDier>
{
    [SerializeField]
    private CinemachineCamera butcherCamera;
    [SerializeField]
    private float killSequenceDuraton;

    public Action KillSequenceEnded;

    public void PlayKillSequence(ButcherController butcher)
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

        butcherCamera.LookAt = butcher.PlayerLookTarget;
        butcherCamera.enabled = true;
        this.CallDelayed(killSequenceDuraton, OnKillSequenceEnded);
    }

    private void OnKillSequenceEnded()
    {
        KillSequenceEnded?.Invoke();
        butcherCamera.LookAt = null;
        butcherCamera.enabled = false;
        PlayerController.Instance.Active = true;

        Hotin.Instance.canDie = true;
        Hotin.Instance.Value = Hotin.Instance.Max;
        Hotin.Instance.canDie = false;
    }
}
