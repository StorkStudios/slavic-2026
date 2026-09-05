using StorkStudios.CoreNest;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

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
    [SerializeField]
    private GameObject pauseScreen;

    public CanvasGroup CameraFadeGroup => cameraFadeGroup;

    private float interactAlfa = 0;
    private float dropAlfa = 0;
    private float exitAlfa = 0;
    private float watchAlfa = 0;
    private float corsairAlfa = 1;

    private void Start()
    {
        pauseScreen.SetActive(false);

        InputAdapter.cancel.performed += OnCancel;
    }

    protected override void OnDestroy()
    {
        InputAdapter.cancel.performed -= OnCancel;
        base.OnDestroy();
    }

    private void OnCancel(InputAction.CallbackContext _)
    {
        if (Minigame.CurrentMinigame != null)
        {
            return;
        }

        SwitchPause();
    }

    public void SwitchPause()
    {
        if (pauseScreen.activeSelf)
        {
            Unpause();
        }
        else
        {
            pauseScreen.SetActive(true);
            PlayerController.Instance.Active = false;
            CursorManager.Instance.UnlockCursor();
            PauseManager.Instance.StartPause();
            InputAdapter.interact.Disable();
            InputAdapter.checkTime.Disable();
            InputAdapter.drop.Disable();
        }
    }

    private void Unpause()
    {
        pauseScreen.SetActive(false);
        PlayerController.Instance.Active = true;
        CursorManager.Instance.LockCursor();
        PauseManager.Instance.StopPause();
        InputAdapter.interact.Enable();
        InputAdapter.checkTime.Enable();
        InputAdapter.drop.Enable();
    }

    public void GoToMainMenu()
    {
        if (pauseScreen.activeSelf)
        {
            Unpause();
        }

        MainMenuController.Win = null;
        SceneManager.LoadScene(SceneEnum.MainMenu.GetBuildIndex());
    }

    private void Update()
    {
        if (PlayerInteractor.Instance.CanInteract)
        {
            interactAlfa = 1;
            interactTooltip.text = $"{PlayerInteractor.Instance.Interactable.ActionName}";
        }
        else if (PlayerObjectHolder.Instance.CurrentObject is PickupableItem item && item.CanUse())
        {
            interactAlfa = 1;
            interactTooltip.text = $"{item.UseName}";
        }
        else
        {
            interactAlfa = 0;
        }

        if (PlayerObjectHolder.Instance.IsHoldingObject && UIPicturesController.Instance.CurrentPicture == null)
        {
            dropAlfa = 1;
            dropTooltip.text = $"{PlayerObjectHolder.Instance.DropActionName}";
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

        interactCanvasGroup.gameObject.SetActive(interactCanvasGroup.alpha > 0);
        dropCanvasGroup.gameObject.SetActive(dropCanvasGroup.alpha > 0);
        exitCanvasGroup.gameObject.SetActive(exitCanvasGroup.alpha > 0);
        watchCanvasGroup.gameObject.SetActive(watchCanvasGroup.alpha > 0);
    }
}
