using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeatPicker : MonoBehaviour
{
    [SerializeField]
    private List<MeatHook> hooks;
    [SerializeField]
    private int meatCount;
    [SerializeField]
    private float nextMeatTime;

    private void Awake()
    {
        GameManager.CallWhenInitialized((gameManager) =>
        {
            gameManager.OnShiftInit += OnShiftInit;
        });
    }

    private void OnShiftInit(ShiftDays.ShiftDay obj)
    {
        hooks[0].Unlock(OnPickup);
        for (int i = 1; i < meatCount; i++)
        {
            hooks.Where(hook => hook.IsLocked).GetRandomElement().Unlock(OnPickup);
        }
    }

    private void OnPickup()
    {
        this.CallDelayed(nextMeatTime, () =>
        {
            if (hooks.Count(hook => !hook.IsLocked) <= 0)
            {
                return;
            }
            hooks.Where(hook => hook.IsLocked).GetRandomElement().Unlock(OnPickup);
        });
    }
}
