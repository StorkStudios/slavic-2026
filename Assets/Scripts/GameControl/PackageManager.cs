using System;
using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(fileName = "PackageManager", menuName = "ScriptableObjects/PackageManager")]
public class PackageManager : ScriptableObjectSingleton<PackageManager>
{
    [SerializeField]
    [ReadOnly]
    private SerializedDictionary<PickupableObject, int> readyPackages;

    [SerializeField]
    private int packagesRequired;
    
    public event ObservableVariable<int>.ValueChangedDelegate CountChanged;
    public event Action AllPackagesReadyEvent;

    public int ReadyPackagesCount => readyPackages.Count;

    public bool AllPackagesReady => ReadyPackagesCount >= packagesRequired;

    public void ResetNoUpdate()
    {
        readyPackages.Clear();
    }

    public void AddPackage(PickupableObject obj)
    {
        readyPackages[obj] = readyPackages.GetValueOrDefault(obj, 0) + 1;
        if (AllPackagesReady)
        {
            AllPackagesReadyEvent?.Invoke();
        }
    }

    public void RemovePackage(PickupableObject obj)
    {
        readyPackages[obj] = readyPackages.GetValueOrDefault(obj, 1) - 1;
        if (readyPackages[obj] <= 0)
        {
            readyPackages.Remove(obj);
        }
    }
}
