using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Product")]
    [SerializeField]
    private GameObject productPrefab;
    [SerializeField]
    private Transform[] productLocations;

    private CursorLockMode lastLockMode;
    private bool started = false;
    protected bool Started => started;
    private PickupableObject currentItem;
    private bool animatingItem = false;

    private void Start()
    {
        cinemachineCamera.enabled = false;
        canvas.gameObject.SetActive(false);
        InputAdapter.cancel.performed += OnCancel;
    }

    private void OnDestroy()
    {
        InputAdapter.cancel.performed -= OnCancel;
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
            animatingItem = true;
            currentItem.transform.DOMove(itemLocation.position, itemAnimtionDuration);
            currentItem.transform.DORotate(itemLocation.eulerAngles, itemAnimtionDuration);
            currentItem.transform.DOScale(itemLocation.localScale, itemAnimtionDuration).OnComplete(() =>
            {
                animatingItem = false;
            });
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
            foreach (Transform productLocation in productLocations)
            {
                Instantiate(productPrefab, productLocation.position, productLocation.rotation);
            }
        }
    }

    public virtual bool CanStart()
    {
        return !started && (currentItem != null || (PlayerObjectHolder.Instance.CurrentObject != null && PlayerObjectHolder.Instance.CurrentObject.ObjectType == itemType));
    }

    public virtual bool CanExit()
    {
        return started && !animatingItem;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (CanExit())
        {
            EndMinigame(false);
        }
    }
}
