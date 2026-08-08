using StorkStudios.CoreNest;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float shiftDuration;
    [ReadOnly]
    public float shiftTime;

    private bool isShift = false;

    private void Start()
    {
        StartShift();
    }

    private void StartShift()
    {
        Hotin.Instance.ResetNoUpdate();
        isShift = true;
    }

    private void Update()
    {
        if (isShift && (shiftTime += Time.deltaTime) > shiftDuration)
        {
            EndShift();
        }
    }

    private void EndShift()
    {
        isShift = false;
    }
}
