using Unity.Cinemachine;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected CinemachineCamera cinemachineCamera;

    private CursorLockMode lastLockMode;

    private void Start()
    {
        cinemachineCamera.enabled = false;
        canvas.gameObject.SetActive(false);
    }
    
    public virtual void StartMinigame()
    {
        cinemachineCamera.enabled = true;
        canvas.gameObject.SetActive(true);
        lastLockMode = Cursor.lockState;
        CursorManager.Instance.UnlockCursor();
    }

    public virtual void EndMinigame()
    {
        cinemachineCamera.enabled = false;
        canvas.gameObject.SetActive(false);
        if (lastLockMode == CursorLockMode.Locked)
        {
            CursorManager.Instance.LockCursor();
        }
    }
}
