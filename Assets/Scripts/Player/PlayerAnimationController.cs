using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool lookingAtWatch = false;

    public void ShowWatch()
    {
        lookingAtWatch = true;
        animator.SetLayerWeight(animator.GetLayerIndex("WatchLayer"), 1);
        animator.CrossFade("WatchLayer.CheckWatch", 0.5f);
    }

    public void HideWatch()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("WatchLayer"), 0);
    }

    public void CrossFade(string stateName, float duration)
    {
        //This is ugly :/
        if (stateName != "Idle" && stateName != "Run")
        {
            HideWatch();
        }
        stateName = "BaseLayer." + stateName;
        animator.CrossFade(stateName, duration);
    }
}
