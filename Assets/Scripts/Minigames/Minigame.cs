using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected Canvas canvas;
    [SerializeField]
    protected CinemachineCamera cinemachineCamera;

    [Header("Item")]
    [SerializeField]
    private Transform itemLocation;
    [SerializeField]
    private string itemType;
    [SerializeField]
    private float itemAnimtionDuration;

    private CursorLockMode lastLockMode;
    private bool started = false;
    private PickupableObject currentItem;

    private void Start()
    {
        cinemachineCamera.enabled = false;
        canvas.gameObject.SetActive(false);
    }
    
    public virtual void StartMinigame()
    {
        cinemachineCamera.enabled = true;
        canvas.gameObject.SetActive(true);
        lastLockMode = Cursor.lockState;
        CursorManager.Instance.UnlockCursor();
        PlayerController.Instance.active = false;
        started = true;
        if (currentItem == null)
        {
            currentItem = PlayerObjectHolder.Instance.DropObject();
            currentItem.GetComponent<Rigidbody>().isKinematic = true;
            currentItem.transform.DOMove(itemLocation.position, itemAnimtionDuration);
            currentItem.transform.DORotate(itemLocation.eulerAngles, itemAnimtionDuration);
            currentItem.transform.DOScale(itemLocation.localScale, itemAnimtionDuration);
        }
    }

    public virtual void EndMinigame(bool win)
    {
        cinemachineCamera.enabled = false;
        canvas.gameObject.SetActive(false);
        if (lastLockMode == CursorLockMode.Locked)
        {
            CursorManager.Instance.LockCursor();
        }
        PlayerController.Instance.active = true;
        started = false;
        if (win)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
        }
    }

    public virtual bool CanStart()
    {
        return !started && (currentItem != null || (PlayerObjectHolder.Instance.CurrentObject != null && PlayerObjectHolder.Instance.CurrentObject.ObjectType == itemType));
    }
}
