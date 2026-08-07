using UnityEngine;

[RequireComponent(typeof(Minigame))]
public class MinigameStation : MonoBehaviour
{
    private Minigame minigame;

    private void Start()
    {
        minigame = GetComponent<Minigame>();
    }

    public void Interact()
    {
        minigame.StartMinigame();
    }
}
