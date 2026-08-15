using System;
using System.Collections.Generic;
using DG.Tweening;
using StorkStudios.CoreNest;
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
    protected Transform itemLocation;
    [SerializeField]
    private string itemType;
    [SerializeField]
    private float itemAnimtionDuration;

    [Header("Product")]
    [SerializeField]
    protected GameObject productPrefab;
    [SerializeField]
    protected Transform[] productLocations;

    private CursorLockMode lastLockMode;
    private bool started = false;
    protected bool Started => started;
    private PickupableObject currentItem;
    protected PickupableObject CurrentItem => currentItem;
    private bool animatingItem = false;

    public abstract string Name { get; }

    private static Minigame currentMinigame;
    public static Minigame CurrentMinigame => currentMinigame;

    [SerializeField]
    [ReadOnly]
    private SerializedSet<GameObject> collidingObjects = new();

    private PickupableObject objectHeldByPlayerOnTable;

    protected virtual void Start()
    {
        cinemachineCamera.enabled = false;
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
        InputAdapter.cancel.performed += OnCancel;
    }

    private void OnDestroy()
    {
        InputAdapter.cancel.performed -= OnCancel;
    }

    public virtual void StartMinigame()
    {
        currentMinigame = this;
        cinemachineCamera.enabled = true;
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
        lastLockMode = Cursor.lockState;
        CursorManager.Instance.UnlockCursor();
        PlayerController.Instance.Active = false;
        started = true;
        if (currentItem == null)
        {
            currentItem = (PickupableObject) PlayerObjectHolder.Instance.DropObject();
            currentItem.locked = true;
            currentItem.GetComponent<Rigidbody>().isKinematic = true;
            animatingItem = true;
            currentItem.transform.DOMove(itemLocation.position, itemAnimtionDuration);
            currentItem.transform.DORotate(itemLocation.eulerAngles, itemAnimtionDuration);
            currentItem.transform.DOScale(itemLocation.localScale, itemAnimtionDuration).OnComplete(() =>
            {
                animatingItem = false;
            });

            collidingObjects.Remove(currentItem.gameObject);
        }
    }

    public virtual void EndMinigame(bool win)
    {
        currentMinigame = null;
        cinemachineCamera.enabled = false;
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
        if (lastLockMode == CursorLockMode.Locked)
        {
            CursorManager.Instance.LockCursor();
        }
        PlayerController.Instance.Active = true;
        started = false;
        if (win)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
            foreach (Transform productLocation in productLocations)
            {
                GameObject obj = Instantiate(productPrefab, productLocation.position, productLocation.rotation);
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                }
                if (obj.TryGetComponent(out PickupableObject _))
                {
                    collidingObjects.Add(obj);
                }
            }
        }
    }

    public virtual bool CanStart()
    {
        return !started &&
            ((currentItem != null && PlayerObjectHolder.Instance.CurrentObject == null) || (PlayerObjectHolder.Instance.CurrentObject != null && PlayerObjectHolder.Instance.CurrentObject.ObjectType == itemType && currentItem == null)) &&
            collidingObjects.Count == 0;
    }

    public virtual bool CanExit()
    {
        return started && !animatingItem;
    }

    public void OnTableTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (!other.attachedRigidbody.TryGetComponent(out PickupableObject obj))
        {
            return;
        }

        if (animatingItem)
        {
            return;
        }

        if ((IPickupable)obj == PlayerObjectHolder.Instance.CurrentObject)
        {
            objectHeldByPlayerOnTable = obj;
            objectHeldByPlayerOnTable.Dropped += OnObjectDroppedOnTable;
        }
        else
        {
            collidingObjects.Add(other.attachedRigidbody.gameObject);
        }
    }

    public void OnTableTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (!other.attachedRigidbody.TryGetComponent(out PickupableObject obj))
        {
            return;
        }
        
        if (obj == objectHeldByPlayerOnTable)
        {
            objectHeldByPlayerOnTable.Dropped -= OnObjectDroppedOnTable;
            objectHeldByPlayerOnTable = null;
        }
        collidingObjects.Remove(other.attachedRigidbody.gameObject);
    }

    private void OnObjectDroppedOnTable(IPickupable pickupable)
    {
        collidingObjects.Add(objectHeldByPlayerOnTable.gameObject);
        objectHeldByPlayerOnTable.Dropped -= OnObjectDroppedOnTable;
        objectHeldByPlayerOnTable = null;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (CanExit())
        {
            EndMinigame(false);
        }
    }

    protected Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, bool pointFilter = false)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
        rt.filterMode = pointFilter ? FilterMode.Point : FilterMode.Bilinear;

        // Set source texture's filter mode too — it affects the blit sampling
        FilterMode originalFilter = source.filterMode;
        source.filterMode = pointFilter ? FilterMode.Point : FilterMode.Bilinear;

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Graphics.Blit(source, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        source.filterMode = originalFilter;

        return result;
    }
}
