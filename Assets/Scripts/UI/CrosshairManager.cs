using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : Singleton<CrosshairManager>
{
    public enum CrosshairType
    {
        Default,
        Blocked,
        Interact
    }

    [SerializeField]
    private SerializedDictionary<CrosshairType, GameObject> crosshairs;



    protected override void Awake()
    {
        base.Awake();

        SetCrosshair(CrosshairType.Default);
    }

    public void SetCrosshair(CrosshairType crosshairType)
    {
        if (crosshairs.TryGetValue(crosshairType, out GameObject sprite))
        {
            foreach (var crosshair in crosshairs.Values)
            {
                crosshair.SetActive(false);
            }
            sprite.SetActive(true);
        }
        else
        {
            Debug.LogError($"No crosshair for type {crosshairType} set in {nameof(CrosshairManager)}");
        }
    }
}
