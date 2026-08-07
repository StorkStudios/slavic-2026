using DG.Tweening;
using UnityEngine;

public class MeatCuttingMarker : MonoBehaviour
{
    [SerializeField]
    private RectTransform timeIndicator;
    [SerializeField]
    private float startScale;
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

    private void StartAnimation()
    {
        animating = true;
        timeIndicator.localScale = Vector3.one * startScale;
        timeIndicator.DOScale(endScale, (startScale - endScale)/startSpeed).OnComplete(() =>
        {
            animating = false;
            MarkerMissed?.Invoke();
            Destroy(gameObject);
        });
    }
}
