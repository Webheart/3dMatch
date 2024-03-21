using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] new Renderer renderer;

    [Header("Material")]
    [SerializeField] Material outlineMaterial;

    private List<Material> originalMaterials;

    void Awake()
    {
        originalMaterials = new List<Material>(renderer.sharedMaterials);
    }

    void OnEnable()
    {
        originalMaterials.Add(outlineMaterial);
        renderer.SetMaterials(originalMaterials);
    }

    void OnDisable()
    {
        originalMaterials.Remove(outlineMaterial);
        renderer.SetMaterials(originalMaterials);
    }
}