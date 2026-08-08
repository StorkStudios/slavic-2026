using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShiftDays", menuName = "ScriptableObjects/ShiftDays")]
public class ShiftDays : ScriptableObjectSingleton<ShiftDays>
{
    [System.Serializable]
    public class ShiftDay
    {

    }

    [SerializeField]
    private List<ShiftDay> shiftDays;

    public ShiftDay CurrentDay => shiftDays[CurrentDayIndex];

    public int CurrentDayIndex
    {
        get => PlayerPrefs.GetInt("currentDayIndex", 0);
        set
        {
            int clamped = Mathf.Clamp(value, 0, shiftDays.Count - 1);
            PlayerPrefs.SetInt("currentDayIndex", clamped);
        }
    }

    public int CurrentDayNumber => CurrentDayIndex + 1;

    public void ResetDays()
    {
        CurrentDayIndex = 0;
    }

    public void NextDay()
    {
        CurrentDayIndex++;
    }
}
