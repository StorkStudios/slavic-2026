using StorkStudios.CoreNest;
using System.Collections;
using TMPro;
using UnityEngine;

public class HotinHandDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
    [SerializeField]
    private SpriteRenderer heart;

    [SerializeField]
    private RangeBoundariesFloat prbpmRange;
    [SerializeField]
    private RangeBoundariesFloat randomOffsetRange;
    [SerializeField]
    private float randomPickTime;
    [SerializeField]
    private Gradient hotinGradient;

    private int offset = 0;
    private bool heartFadeIn = true;

    private void Start()
    {
        StartCoroutine(RandomOffsetPickerCoroutine());
    }

    private IEnumerator RandomOffsetPickerCoroutine()
    {
        while (true)
        {
            offset = (int) randomOffsetRange.GetRandomBetween();
            yield return new WaitForSeconds(randomPickTime);
        }
    }

    private void Update()
    {
        float t = Hotin.Instance.NormalizedValue;
        float currentPrbpm = Mathf.Lerp(prbpmRange.Min, prbpmRange.Max, t);
        int displayValue = (int) currentPrbpm + offset;
        text.text = displayValue.ToString();

        Color color = hotinGradient.Evaluate(t);
        float speed = currentPrbpm / 30;
        float target = heartFadeIn ? 1 : 0;
        color.a = Mathf.MoveTowards(heart.color.a, target, speed * Time.deltaTime);
        if (color.a == target)
        {
            heartFadeIn = !heartFadeIn;
        }
        heart.color = color;
    }
}
