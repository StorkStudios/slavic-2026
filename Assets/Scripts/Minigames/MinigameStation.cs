using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameStation : MonoBehaviour, IInteractable
{
    private Minigame minigame;

    string IInteractable.ActionName => minigame.Name;

    private void Start()
    {
        minigame = GetComponent<Minigame>();
    }

    public bool CanInteract()
    {
        return minigame.CanStart();
    }

    public void Interact()
    {
        minigame.StartMinigame();
    }
}
