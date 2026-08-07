using Unity.Cinemachine;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected CinemachineCamera cinemachineCamera;

    private CursorLockMode lastLockMode;
    
    public virtual void StartMinigame()
    {
        cinemachineCamera.enabled = true;
        lastLockMode = Cursor.lockState;
        CursorManager.Instance.UnlockCursor();
    }

    public virtual void EndMinigame()
    {
        cinemachineCamera.enabled = false;
        if (lastLockMode == CursorLockMode.Locked)
        {
            CursorManager.Instance.LockCursor();
        }
    }
}
