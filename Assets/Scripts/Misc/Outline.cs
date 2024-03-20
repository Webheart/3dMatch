using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] new Renderer renderer;

    [Header("Material")]
    [SerializeField] Material outlineMaterial;

    private Material materialInstance;
    private List<Material> originalMaterials;

    void Awake()
    {
        materialInstance = Instantiate(outlineMaterial);
        originalMaterials = new List<Material>(renderer.sharedMaterials);
    }

    void OnEnable()
    {
        originalMaterials.Add(materialInstance);
        renderer.SetMaterials(originalMaterials);
    }

    void OnDisable()
    {
        originalMaterials.Remove(materialInstance);
        renderer.SetMaterials(originalMaterials);
    }

    void OnDestroy()
    {
        Destroy(materialInstance);
    }
}