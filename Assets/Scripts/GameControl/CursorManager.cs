using UnityEngine;
using StorkStudios.CoreNest;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField]
    private bool preventCursorLocking = false;

    protected override void Awake()
    {
        if (preventCursorLocking)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }

        base.Awake();
    }

    public void LockCursor()
    {
        if (!preventCursorLocking)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
