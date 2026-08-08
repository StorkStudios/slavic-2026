using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShowablePicture : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string actionName;
    [SerializeField]
    private string cancelActionName;
    [SerializeField]
    private string pictureToShow;
    [SerializeField]
    private string showSound;
    [SerializeField]
    private string hideSound;

    public string ActionName => IsShowing ? cancelActionName : actionName;
    public string InteractSound => IsShowing ? hideSound : showSound;

    private bool IsShowing => UIPicturesController.Instance.CurrentPicture == pictureToShow;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (IsShowing)
        {
            UIPicturesController.Instance.HideCurrent();
            PlayerController.Instance.active = true;
        }
        else
        {
            UIPicturesController.Instance.ShowPicture(pictureToShow);
            PlayerController.Instance.active = false;
        }
    }
}
