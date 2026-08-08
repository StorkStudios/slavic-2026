using UnityEngine;

public class BloodController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve remapNormalizedHotinToScale;
    [SerializeField]
    private AudioClip bloodWalk;
    [SerializeField]
    private AudioClip bloodRun;

    private AudioSource walkSource;
    private AudioSource runSource;

    private AudioClip normalWalk;
    private AudioClip normalRun;

    private void Start()
    {
        Hotin.Instance.ValueChanged += OnHotinChanged;
        SetYScale(0);

        walkSource = PlayerController.Instance.WalkSound;
        runSource = PlayerController.Instance.SprintSound;

        normalWalk = walkSource.clip;
        normalRun = runSource.clip;
    }

    private void OnDestroy()
    {
        Hotin.Instance.ValueChanged -= OnHotinChanged;
    }

    private void OnHotinChanged(float oldValue, float newValue)
    {
        float normalizedValue = newValue / Hotin.Instance.Max;
        normalizedValue = remapNormalizedHotinToScale.Evaluate(normalizedValue);
        SetYScale(normalizedValue);
    }

    private void SetYScale(float y)
    {
        Vector3 scale = transform.localScale;
        scale.y = y;
        transform.localScale = scale;
    }

    public void SwapSounds(bool blood)
    {
        if (walkSource.isPlaying)
        {
            walkSource.clip = blood ? bloodWalk : normalWalk;
            walkSource.Play();
        }
        else
        {
            walkSource.clip = blood ? bloodWalk : normalWalk;
        }

        if (runSource.isPlaying)
        {
            runSource.clip = blood ? bloodRun : normalRun;
            runSource.Play();
        }
        else
        {
            runSource.clip = blood ? bloodRun : normalRun;
        }
    }
}
