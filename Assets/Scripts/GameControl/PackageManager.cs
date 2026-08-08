using System;
using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(fileName = "PackageManager", menuName = "ScriptableObjects/PackageManager")]
public class PackageManager : ScriptableObjectSingleton<PackageManager>
{
    [SerializeField]
    [ReadOnly]
    private int readyPackages;
    
    public event ObservableVariable<int>.ValueChangedDelegate ValueChanged;
    public event Action AllPackagesReadyEvent;

    public int ReadyPackages
    {
        get => readyPackages;
        set
        {
            if (value != readyPackages)
            {
                int oldValue = readyPackages;
                readyPackages = value;
                ValueChanged?.Invoke(readyPackages, value);
                if (packagesRequired == readyPackages)
                {
                    AllPackagesReadyEvent?.Invoke();
                }
            }
        }
    }

    public bool AllPackagesReady => readyPackages >= packagesRequired;
    
    [SerializeField]
    private int packagesRequired;

    public void ResetNoUpdate()
    {
        readyPackages = 0;
    }
}
