using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameStation : MonoBehaviour, IInteractable
{
    private Minigame minigame;

    private void Start()
    {
        minigame = GetComponent<Minigame>();
    }

    public void Interact(GameObject playerObject)
    {
        minigame.StartMinigame();
    }
}
