using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(fileName = "Hotin", menuName = "ScriptableObjects/Hotin")]
public class Hotin : ScriptableObjectSingleton<Hotin>
{
    [SerializeField]
    [ReadOnly]
    private ObservableVariable<float> hotinValue;
    public ObservableVariable<float> HotinValue => hotinValue;
}
