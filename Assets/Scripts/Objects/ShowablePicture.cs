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
    [SerializeField]
    private bool destroyOnPutDown;

    public string ActionName => IsShowing ? cancelActionName : actionName;
    public string InteractSound => IsShowing ? hideSound : showSound;

    private bool IsShowing => UIPicturesController.Instance.CurrentPicture == pictureToShow;

    private void Start()
    {
        UIPicturesController.Instance.PictureHid += OnPictureHid;
    }

    private void OnDestroy()
    {
        if (UIPicturesController.IsInstanced)
        {
            UIPicturesController.Instance.PictureHid -= OnPictureHid;
        }
    }

    private void OnPictureHid(string picture)
    {
        if (destroyOnPutDown && pictureToShow == picture)
        {
            Destroy(gameObject);
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (IsShowing)
        {
            UIPicturesController.Instance.HideCurrent();
            PlayerController.Instance.Active = true;
        }
        else
        {
            UIPicturesController.Instance.ShowPicture(pictureToShow);
            PlayerController.Instance.Active = false;
        }
    }
}
