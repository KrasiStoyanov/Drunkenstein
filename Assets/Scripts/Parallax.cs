using System.Collections.Generic;

using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Camera mainCamera;

    [Tooltip("The global scroll speed of the parallax.")]
    public float scrollSpeed;

    [Tooltip("The list of all the layers in the parallax.")]
    public List<Layer> layers;

    [Tooltip("The list of all the layers in the parallax.")]
    public DistanceCounter distanceManager;

    private float initialScrollSpeed = 6.0f;
    private float maxScrollSpeed = 31.6f;
    private float scrollSpeedRate = 1.2f;

    /// <summary>
    /// Setup the layers.
    /// </summary>
    public void Start()
    {
        mainCamera = Camera.main;
        foreach (Layer layer in layers)
        {
            layer.Spawn(0f);

            CheckPosition(layer);
        }
    }

    public void Initialize()
    {
        scrollSpeed = initialScrollSpeed;
    }

    /// <summary>
    /// Update the layers.
    /// </summary>
    public void Update()
    {
        float time = Time.deltaTime * scrollSpeed;
        foreach (Layer layer in layers)
        {
            CheckPosition(layer);

            layer.Update(mainCamera, time);
        }

        Layer firstLayer = layers[0];
        Renderer spriteRenderer = firstLayer.GetSpriteRendererAtIndex(0);

        bool shouldUpdateDistanceMeter = false;
        shouldUpdateDistanceMeter = firstLayer.UpdateDistance(spriteRenderer);
        if (shouldUpdateDistanceMeter)
        {
            distanceManager.UpdateDistance();
        }
    }

    private void CheckPosition(Layer layer)
    {
        GameObject spriteToDestroy = layer.CheckPosition(mainCamera);
        if (spriteToDestroy != null)
        {
            Destroy(spriteToDestroy);
        }
    }

    public void ChangeSpeed(float sliderValue)
    {
        // TODO: Gradually change speed using Mathf.Lerp
        //scrollSpeed = Mathf.Lerp(scrollSpeed, initialScrollSpeed + (scrollSpeedRate * sliderValue), 5.0f);
        scrollSpeed = initialScrollSpeed + (scrollSpeedRate * sliderValue);
        scrollSpeed = Mathf.Clamp(scrollSpeed, initialScrollSpeed, maxScrollSpeed);
    }
}
