using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMain : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Material material;
    [SerializeField] private RawImage rawImage;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
