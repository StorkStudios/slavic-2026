using Unity.Cinemachine;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected CinemachineCamera cinemachineCamera;
    
    public virtual void StartMinigame()
    {
        cinemachineCamera.enabled = true;
        
    }

    public virtual void EndMinigame()
    {
        cinemachineCamera.enabled = false;
    }
}
