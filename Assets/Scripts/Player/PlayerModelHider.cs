using UnityEngine;

public class PlayerModelHider : MonoBehaviour
{
    [SerializeField]
    private GameObject playerModel;

    private void Update()
    {
        if (Minigame.CurrentMinigame == null)
        {
            if (!playerModel.activeSelf)
            {
                playerModel.SetActive(true);
            }
        }
        else
        {
            if (playerModel.activeSelf)
            {
                playerModel.SetActive(false);
            }
        }
    }
}
