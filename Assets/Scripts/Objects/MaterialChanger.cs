using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialChanger : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<string, List<Material>> materialsPresets;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterials(string preset)
    {
        renderer.SetMaterials(materialsPresets[preset]);
    }
}
