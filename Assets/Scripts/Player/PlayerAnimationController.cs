using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool lookingAtWatch = false;
    private int watchLayerIndex;
    private TweenerCore<float, float, FloatOptions> tween;

    private void Awake()
    {
        watchLayerIndex = animator.GetLayerIndex("WatchLayer");
    }

    public void ShowWatch()
    {
        if (tween != null && tween.active)
        {
            tween.Kill();
        }
        lookingAtWatch = true;
        tween = DOTween.To(() => animator.GetLayerWeight(watchLayerIndex), x => animator.SetLayerWeight(watchLayerIndex, x), 1, 0.5f);
        animator.CrossFade("WatchLayer.CheckWatch", 0);
    }

    public void HideWatch()
    {
        if (tween != null && tween.active)
        {
            tween.Kill();
        }
        lookingAtWatch = false;
        DOTween.To(() => animator.GetLayerWeight(watchLayerIndex), x => animator.SetLayerWeight(watchLayerIndex, x), 0, 0.5f);
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
