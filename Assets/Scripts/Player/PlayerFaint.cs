using DG.Tweening;
using StorkStudios.CoreNest;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerFaint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("I am a little bit of loneliness, a little bit of disregard\r\nHandful of complaints, but I can't help the fact that everyone can see these scars\r\nI am what I want you to want, what I want you to feel\r\nBut it's like, no matter what I do, I can't convince you to just believe this is real\r\nSo I let go, watching you\r\nTurn your back like you always do\r\nFace away and pretend that I'm not\r\nBut I'll be here 'cause you're all that I got\r\nI can't feel the way I did before\r\nDon't turn your back on me, I won't be ignored\r\nTime won't heal this damage anymore\r\nDon't turn your back on me, I won't be ignored\r\nI am a little bit insecure, a little unconfident\r\n'Cause you don't understand, I do what I can, but sometimes I don't make sense\r\nI am what you never want to say, but I've never had a doubt\r\nIt's like, no matter what I do, I can't convince you for once just to hear me out\r\nSo I let go, watching you\r\nTurn your back like you always do\r\nFace away and pretend that I'm not\r\nBut I'll be here 'cause you're all that I got\r\nI can't feel the way I did before\r\nDon't turn your back on me, I won't be ignored\r\nTime won't heal this damage anymore\r\nDon't turn your back on me, I won't be ignored\r\nNo, hear me out now\r\nYou're gonna listen to me, like it or not\r\nRight now, hear me out now\r\nYou're gonna listen to me, like it or not\r\nRight now (I can't feel the way I did before)\r\nDon't turn your back on me, I won't be ignored\r\nI can't feel the way I did before\r\nDon't turn your back on me, I won't be ignored\r\nTime won't heal this damage anymore\r\nDon't turn your back on me, I won't be ignored\r\nI can't feel\r\nDon't turn your back on me, I won't be ignored\r\nTime won't heal\r\nDon't turn your back on me, I won't be ignored")]
    private float faintAnimationDuration;
    [SerializeField]
    private float faintDuration;
    [SerializeField]
    private float shiftTimeAddOnFaint;
    [SerializeField]
    private float hotinReduceValue;
    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    private void Start()
    {
        cinemachineCamera.enabled = false;
        Hotin.Instance.ValueChanged += OnHotinChange;
    }

    private void OnDestroy()
    {
        Hotin.Instance.ValueChanged -= OnHotinChange;
    }

    private void OnHotinChange(float oldValue, float newValue)
    {
        if (newValue >= Hotin.Instance.Max)
        {
            Faint();
        }
    }

    private void Faint()
    {
        if (Minigame.CurrentMinigame != null)
        {
            Minigame.CurrentMinigame.EndMinigame(false);
        }

        Debug.Log("I've become so numb");

        cinemachineCamera.enabled = true;
        PlayerController.Instance.Active = false;
        HUDController.Instance.CameraFadeGroup.DOFade(1, faintAnimationDuration);

        this.CallDelayed(faintAnimationDuration + faintDuration, UnFaint);
    }

    private void UnFaint()
    {
        Hotin.Instance.Value -= hotinReduceValue;
        GameManager.Instance.shiftTime += shiftTimeAddOnFaint;
        cinemachineCamera.enabled = false;
        PlayerController.Instance.Active = true;
        HUDController.Instance.CameraFadeGroup.DOFade(0, faintAnimationDuration);
    }
}
