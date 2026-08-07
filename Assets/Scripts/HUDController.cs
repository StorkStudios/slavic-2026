using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactTooltip;
    [SerializeField]
    private CanvasGroup interactCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI dropTooltip;
    [SerializeField]
    private CanvasGroup dropCanvasGroup;
    [SerializeField]
    private float showDuration;

    private float interactAlfa = 0;
    private float dropAlfa = 0;

    private void Update()
    {
        if (PlayerInteractor.Instance.CanInteract)
        {
            interactAlfa = 1;
            interactTooltip.text = $"[{InputAdapter.interact.GetBindingDisplayString(group: "Keyboard&Mouse")}] {PlayerInteractor.Instance.Interactable.ActionName}";
        }
        else
        {
            interactAlfa = 0;
        }

        if (PlayerObjectHolder.Instance.IsHoldingObject)
        {
            dropAlfa = 1;
            dropTooltip.text = $"[{InputAdapter.drop.GetBindingDisplayString(group: "Keyboard&Mouse")}] {PlayerObjectHolder.Instance.DropActionName}";
        }
        else
        {
            dropAlfa = 0;
        }

        interactCanvasGroup.alpha = Mathf.MoveTowards(interactCanvasGroup.alpha, interactAlfa, 1 / showDuration);
        dropCanvasGroup.alpha = Mathf.MoveTowards(dropCanvasGroup.alpha, dropAlfa, 1 / showDuration);

        interactCanvasGroup.gameObject.SetActive(interactCanvasGroup.alpha > 0);
        dropCanvasGroup.gameObject.SetActive(dropCanvasGroup.alpha > 0);
    }
}
