using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameStation : MonoBehaviour, IInteractable
{
    private Minigame minigame;

    private void Start()
    {
        minigame = GetComponent<Minigame>();
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        minigame.StartMinigame();
    }
}
