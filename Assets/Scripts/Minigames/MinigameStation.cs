using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameStation : MonoBehaviour, IInteractable
{
    private Minigame minigame;

    string IInteractable.ActionName => "play " + minigame.GetType().Name;

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
