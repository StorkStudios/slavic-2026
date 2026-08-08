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

    [Header("Events")]
    [SerializeField]
    private int hotinEventThreshold;
    [SerializeField]
    private float grindEventTimeout;
    public Trigger grindHotinEvent;

    public override string Name => "Grind";

    private float rotatedAngle = 0f;
    private bool pressed = false;
    private float lastAngle;
    private Vector2 screenHalf;
    private float lastParticleTime = 0f;
    private float grindStopTimestamp = 0f;
    private float lastEventTimestamp;
    private bool grindingLastFrame;

    public override void StartMinigame()
    {
        base.StartMinigame();
        screenHalf = new Vector2(Screen.width / 2f, Screen.height / 2f);

        InputAdapter.interact.started += OnMousePress;
        InputAdapter.interact.canceled += OnMouseRelease;
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        InputAdapter.interact.started -= OnMousePress;
        InputAdapter.interact.canceled -= OnMouseRelease;
        rotatedAngle = 0f;

        if (particles != null && particles.isPlaying)
        {
            particles.Stop();
        }

        grindingAudioSource.Stop();
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
            if (particles != null)
            {
                if (!particles.isPlaying)
                {
                    particles.Play();
                }
                lastParticleTime = Time.time;
            }

            if (Hotin.Instance.Value > hotinEventThreshold)
            {
                if (lastEventTimestamp + grindEventTimeout < Time.time)
                {
                    grindHotinEvent.Invoke();
                    lastEventTimestamp = Time.time + grindEventTimeout;
                }
            }

            grinding = diff > 0;
            if (!grindingAudioSource.isPlaying && grinding)
            {
                grindingAudioSource.Play();
            }

            handle.localRotation = Quaternion.Euler(0f, 0f, 360f - angle);
            rotatedAngle += diff;
            if (rotatedAngle >= grindAngle)
            {
                EndMinigame(true);
            }
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
    }

    private void OnMousePress(InputAction.CallbackContext context)
    {
        pressed = true;
        lastAngle = GetMouseAngle();
    }

    private float GetMouseAngle()
    {
        return Vector2.SignedAngle(Vector2.up, Mouse.current.position.ReadValue() - screenHalf) + 180f;
    }
}
