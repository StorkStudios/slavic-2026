using StorkStudios.CoreNest;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUDController : Singleton<HUDController>
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
    private TextMeshProUGUI exitTooltip;
    [SerializeField]
    private CanvasGroup exitCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI watchTooltip;
    [SerializeField]
    private CanvasGroup watchCanvasGroup;
    [SerializeField]
    private float showDuration;
    [SerializeField]
    private CanvasGroup cameraFadeGroup;
    [SerializeField]
    private CanvasGroup corsairGroup;

    public CanvasGroup CameraFadeGroup => cameraFadeGroup;

    private float interactAlfa = 0;
    private float dropAlfa = 0;
    private float exitAlfa = 0;
    private float watchAlfa = 0;
    private float corsairAlfa = 1;

    private void Update()
    {
        if (PlayerInteractor.Instance.CanInteract)
        {
            interactAlfa = 1;
            interactTooltip.text = $"[{InputAdapter.interact.GetBindingDisplayString(group: "Keyboard&Mouse")}] {PlayerInteractor.Instance.Interactable.ActionName}";
        }
        else if (PlayerObjectHolder.Instance.CurrentObject is PickupableItem item && item.CanUse())
        {
            interactAlfa = 1;
            interactTooltip.text = $"[{InputAdapter.interact.GetBindingDisplayString(group: "Keyboard&Mouse")}] {item.UseName}";
        }
        else
        {
            interactAlfa = 0;
        }

        if (PlayerObjectHolder.Instance.IsHoldingObject && UIPicturesController.Instance.CurrentPicture == null)
        {
            dropAlfa = 1;
            dropTooltip.text = $"[{InputAdapter.drop.GetBindingDisplayString(group: "Keyboard&Mouse")}] {PlayerObjectHolder.Instance.DropActionName}";
        }
        else
        {
            dropAlfa = 0;
        }

        if (Minigame.CurrentMinigame != null)
        {
            exitAlfa = 1;
            exitTooltip.text = $"[{InputAdapter.cancel.GetBindingDisplayString(group: "Keyboard&Mouse")}] Cancel";
        }
        else
        {
            exitAlfa = 0;
        }

        if (PlayerWatchWatcher.Instance.CanCheckTime() || PlayerWatchWatcher.Instance.Watching)
        {
            watchAlfa = 1;
            watchTooltip.text = $"[{InputAdapter.checkTime.GetBindingDisplayString(group: "Keyboard&Mouse")}] {(PlayerWatchWatcher.Instance.Watching ? "Hide watch" : "Show watch")}";
        }
        else
        {
            watchAlfa = 0;
        }

        if ((PlayerObjectHolder.Instance.IsHoldingObject && PlayerObjectHolder.Instance.CurrentObject.ObjectType == "dart") || Minigame.CurrentMinigame != null)
        {
            corsairAlfa = 0;
        }
        else
        {
            corsairAlfa = 1;
        }

        interactCanvasGroup.alpha = Mathf.MoveTowards(interactCanvasGroup.alpha, interactAlfa, 1 / showDuration);
        dropCanvasGroup.alpha = Mathf.MoveTowards(dropCanvasGroup.alpha, dropAlfa, 1 / showDuration);
        exitCanvasGroup.alpha = Mathf.MoveTowards(exitCanvasGroup.alpha, exitAlfa, 1 / showDuration);
        watchCanvasGroup.alpha = Mathf.MoveTowards(watchCanvasGroup.alpha, watchAlfa, 1 / showDuration);
        corsairGroup.alpha = Mathf.MoveTowards(corsairGroup.alpha, corsairAlfa, 1 / showDuration);
    }
}
