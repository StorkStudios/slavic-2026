using StorkStudios.CoreNest;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        Hotin.Instance.HotinValue.Value = 0;
    }
}
