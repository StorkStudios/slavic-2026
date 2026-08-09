using System;
using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BloodSplashController : MonoBehaviour
{
    [SerializeField]
    private List<Material> materials;
    [SerializeField]
    private RangeBoundariesFloat hotinThreshholdRange;

    [SerializeField]
    [ReadOnly]
    private float hotinThreshhold;

    private MeshRenderer renderer;


    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
        hotinThreshhold = hotinThreshholdRange.GetRandomBetween();

        Hotin.Instance.ValueChanged += OnHotinValueChanged;
        Hide();
    }

    private void OnHotinValueChanged(float oldValue, float newValue)
    {
        if (newValue > hotinThreshhold && oldValue <= hotinThreshhold)
        {
            Show();
        }
        else if (oldValue >= hotinThreshhold && newValue < hotinThreshhold)
        {
            Hide();
        }
    }

    public void Show()
    {
        renderer.enabled = true;
        renderer.material = materials.GetRandomElement();
    }

    public void Hide()
    {
        renderer.enabled = false;
    }
}