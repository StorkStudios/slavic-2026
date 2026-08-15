using UnityEngine;
using StorkStudios.CoreNest;

public class PauseManager : Singleton<PauseManager>
{
    public bool IsPaused => isPaused;
    public event System.Action<bool> pauseStateChanged;

    [SerializeField]
    [ReadOnly]
    private bool isPaused = false;
    [SerializeField]
    [ReadOnly]
    private int pauseLock = 0;

    public void StartPause()
    {
        pauseLock++;
        if (pauseLock == 1)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseStateChanged?.Invoke(IsPaused);
        }
    }

    public void StopPause()
    {
        pauseLock--;
        if (pauseLock == 0)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseStateChanged?.Invoke(IsPaused);
        }
    }
}
