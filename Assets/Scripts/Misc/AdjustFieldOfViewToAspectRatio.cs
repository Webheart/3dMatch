using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AdjustFieldOfViewToAspectRatio : MonoBehaviour
{
    public float2 BaseAspectRatio;
    public int BaseFOV;

    private void Awake()
    {
        var camera = GetComponent<Camera>();

        var newFoV = BaseFOV * ((BaseAspectRatio.x / BaseAspectRatio.y) / camera.aspect);
        camera.fieldOfView = newFoV;
    }
}