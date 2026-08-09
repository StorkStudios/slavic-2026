using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float shiftDuration;
    public float ShiftDuration => shiftDuration;

    [ReadOnly]
    public float shiftTime;

    public event System.Action<ShiftDays.ShiftDay> OnShiftInit;

    private bool isShift = false;

    private void Start()
    {
        InitShift(ShiftDays.Instance.CurrentDay);
        StartShift();
    }

    private void InitShift(ShiftDays.ShiftDay shiftDay)
    {
        Hotin.Instance.ResetNoUpdate();
        Hotin.Instance.currentGainMultiplier = shiftDay.hotinGainMultiplier;
        OnShiftInit?.Invoke(shiftDay);
        PackageManager.Instance.AllPackagesReadyEvent += OnAllPackagesReady;
        PackageManager.Instance.ResetNoUpdate();
    }

    private void OnAllPackagesReady()
    {
        EndShift();
    }

    private void StartShift()
    {
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
        if (CheckWin())
        {
            ShiftDays.Instance.NextDay();
        }
        SceneManager.LoadScene(SceneEnum.MainMenu.GetBuildIndex());
    }

    private bool CheckWin()
    {
        return PackageManager.Instance.AllPackagesReady;
    }
}
