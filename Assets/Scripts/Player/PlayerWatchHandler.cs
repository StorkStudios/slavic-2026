using System;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class PlayerWatchHandler : MonoBehaviour
{
    private TMPro.TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
    }

    private void Update()
    {
        float shiftNormlizedTime = GameManager.Instance.shiftTime / GameManager.Instance.ShiftDuration;
        int watchTime = (int)(shiftNormlizedTime * 8f * 60f) + 22 * 60;
        text.text = $"{(watchTime/60 % 24).ToString().PadLeft(2, '0')}\n{(watchTime%60).ToString().PadLeft(2, '0')}";
    }
}
