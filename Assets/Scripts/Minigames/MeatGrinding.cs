using System;
using DG.Tweening;
using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeatGrinding : Minigame
{
    [Header("Settings")]
    [SerializeField]
    private float grindAngle;
    [SerializeField]
    private float particlesTimeout;
    [SerializeField]
    private float soundTimeout;

    [Header("References")]
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private AudioSource grindingAudioSource;
    [SerializeField]
    private Transform finalMeatLocation;
    [SerializeField]
    private Transform initialProductLocation;
    [SerializeField]
    private GameObject fakeMeatPrefab;
    [SerializeField]
    private Sprite cursorSpriteOpen;
    [SerializeField]
    private Sprite cursorSpriteClosed;

    [Header("Events")]
    [SerializeField]
    private int hotinEventThreshold;
    [SerializeField]
    private float grindEventTimeout;
    public Trigger grindHotinEvent;

    [Header("Tip")]
    [SerializeField]
    private GameObject tableHint;

    public override string Name => "Grind";

    private float totalRotatedAngle = 0f;
    private bool pressed = false;
    private float lastAngle;
    private Vector2 screenHalf;
    private float lastParticleTime = 0f;
    private float grindStopTimestamp = 0f;
    private float lastEventTimestamp;
    private bool grindingLastFrame;
    private GameObject fakeProduct;

    private static Texture2D scaledCursorOpen;
    private static Texture2D scaledCursorClosed;

    protected override void Start()
    {
        base.Start();

        if (scaledCursorOpen == null)
        {
            scaledCursorOpen = ScaleTexture(cursorSpriteOpen.texture, 32, 32);
        }
        if (scaledCursorClosed == null)
        {
            scaledCursorClosed = ScaleTexture(cursorSpriteClosed.texture, 32, 32);
        }
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        tableHint.gameObject.SetActive(true);

        screenHalf = new Vector2(Screen.width / 2f, Screen.height / 2f);

        InputAdapter.interact.started += OnMousePress;
        InputAdapter.interact.canceled += OnMouseRelease;

        if (fakeProduct == null)
        {
            fakeProduct = Instantiate(fakeMeatPrefab, initialProductLocation.position, initialProductLocation.rotation);
        }

        Cursor.SetCursor(scaledCursorOpen, Vector2.zero, CursorMode.Auto);
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        tableHint.gameObject.SetActive(false);

        if (win)
        {
            Destroy(fakeProduct);
            fakeProduct = null;
            totalRotatedAngle = 0f;
            handle.localRotation = Quaternion.identity;
        }

        InputAdapter.interact.started -= OnMousePress;
        InputAdapter.interact.canceled -= OnMouseRelease;

        if (particles != null && particles.isPlaying)
        {
            particles.Stop();
        }

        grindingAudioSource.Stop();

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (!Started)
        {
            return;
        }

        float angle = GetMouseAngle();
        bool grinding = false;

        if (pressed)
        {
            float diff = GetAngleDelta(angle, lastAngle);
            grinding = diff > 0;

            if (grinding)
            {
                if (Hotin.Instance.Value > hotinEventThreshold)
                {
                    if (lastEventTimestamp + grindEventTimeout < Time.time)
                    {
                        grindHotinEvent.Invoke();
                        lastEventTimestamp = Time.time + grindEventTimeout;
                    }
                }

                if (!grindingAudioSource.isPlaying)
                {
                    grindingAudioSource.Play();
                }

                if (particles != null)
                {
                    if (!particles.isPlaying)
                    {
                        particles.Play();
                    }
                    lastParticleTime = Time.time;
                }

                handle.localRotation = Quaternion.Euler(0f, 0f, 360f - angle);
            }

            totalRotatedAngle += diff;
            if (totalRotatedAngle >= grindAngle)
            {
                EndMinigame(true);
            }
        }

        if (Started) //May be false after end minigame
        {
            float grindProgress = totalRotatedAngle / grindAngle;
            CurrentItem.transform.Lerp(itemLocation, finalMeatLocation, grindProgress);
            fakeProduct.transform.Lerp(initialProductLocation, productLocations[0], grindProgress);
        }

        if (grindingLastFrame && !grinding)
        {
            grindStopTimestamp = Time.time;
        }

        if (!grinding && grindStopTimestamp + soundTimeout > Time.time)
        {
            grindingAudioSource.Stop();
        }

        grindingLastFrame = grinding;
        lastAngle = angle;

        if (particles != null && particles.isPlaying && Time.time - lastParticleTime > particlesTimeout)
        {
            particles.Stop();
        }
    }

    private float GetAngleDelta(float currentAngle, float lastAngle)
    {
        if (lastAngle - currentAngle > 0f && lastAngle - currentAngle < 180f)
        {
            return lastAngle - currentAngle;
        }
        if (currentAngle > 270 && lastAngle < 90)
        {
            return currentAngle - 360 - lastAngle;
        }
        return 0f;
    }

    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        pressed = false;
        Cursor.SetCursor(scaledCursorOpen, Vector2.zero, CursorMode.Auto);
    }

    private void OnMousePress(InputAction.CallbackContext context)
    {
        pressed = true;
        lastAngle = GetMouseAngle();
        Cursor.SetCursor(scaledCursorClosed, Vector2.zero, CursorMode.Auto);
    }

    private float GetMouseAngle()
    {
        return Vector2.SignedAngle(Vector2.up, Mouse.current.position.ReadValue() - screenHalf) + 180f;
    }
}
