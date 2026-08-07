using DG.Tweening;
using UnityEngine;

public class MeatCuttingMarker : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeIndicator;
    [SerializeField]
    private float animationStartScale;
    [SerializeField]
    private float endScale;
    [SerializeField]
    private float startSpeed;
    [SerializeField]
    private float correctScaleRange;
    
    public event System.Action MarkerCut;
    public event System.Action MarkerMissed;
    private bool animating = false;

    private void Start()
    {
        StartAnimation();
    }

    public void OnCut()
    {
        if (animating)
        {
            float currentScale = timeIndicator.localScale.x;
            if (Mathf.Abs(1 - currentScale) <= correctScaleRange)
            {
                MarkerCut?.Invoke();
            }
            else
            {
                MarkerMissed?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        timeIndicator.DOKill();
    }

    private void StartAnimation()
    {
        animating = true;
        timeIndicator.localScale = Vector3.one * animationStartScale;
        timeIndicator.DOScale(endScale, (animationStartScale - endScale) / startSpeed).OnComplete(() =>
        {
            animating = false;
            MarkerMissed?.Invoke();
        });
    }
}
