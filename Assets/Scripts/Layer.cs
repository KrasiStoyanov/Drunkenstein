using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Layer
{
    private List<Renderer> spriteRenderers = new List<Renderer>();

    [Tooltip("The list of the different sprite variations of the current layer.")]
    public List<GameObject> sprites;

    [Tooltip("The speed at which this object moves in relation to the speed of the parallax.")]
    [Range(0.0f, 1.0f)]
    public float speedRatio;

    private const float widthOfSprite = 19.2f;
    private float previousSpritePosition = 0f;

    /// <summary>
    /// Spawn a new sprite in the map.
    /// </summary>
    /// <param name="xPosition">The X position at which the sprite should be spawn.</param>
    public void Spawn(float xPosition)
    {
        // The limit of each layer is 2 sprite renderers - one original, one copy
        if (spriteRenderers.Count >= 2)
        {
            return;
        }

        // Get random index if there are more than 1 sprites in this current layer
        int index = 0;
        if (sprites.Count > 1)
        {
            index = UnityEngine.Random.Range(0, sprites.Count);
        }

        // Instantiate a new sprite and get its renderer component
        GameObject spawnSprite = GameObject.Instantiate(sprites[index]) as GameObject;
        Renderer spriteRenderer = spawnSprite.GetComponent<Renderer>();

        // Position the newly created sprite
        spawnSprite.transform.parent = sprites[index].transform.parent;
        spawnSprite.transform.position = new Vector3(xPosition, 0f, 0f);

        // Show the sprite renderer
        spriteRenderer.enabled = true;

        // Add the current sprite renderer to the list of sprite renderers
        spriteRenderers.Add(spriteRenderer);
    }

    /// <summary>
    /// Check the position of the sprite renderers.
    /// </summary>
    /// <param name="camera">The main camera.</param>
    public GameObject CheckPosition(Camera camera)
    {
        GameObject spriteToDestroy = null;
        if (spriteRenderers.Count > 0)
        {
            float cameraXPosition = camera.transform.position.x;
            foreach (Renderer spriteRenderer in spriteRenderers.ToArray())
            {
                // if the right side of the current sprite renderer is at the right side of the camera, spawn a new sprite outside the camera on the right side
                if (spriteRenderer.transform.position.x == cameraXPosition)
                {
                    Spawn(widthOfSprite);
                }

                // if the right side of the current sprite renderer is at the left side of the camera, remove the sprite renderer from the list, spawn a new sprite and destroy the gameobject
                if (spriteRenderer.transform.position.x <= -widthOfSprite)
                {
                    float newPosition = spriteRenderer.transform.position.x + (widthOfSprite * 2);

                    spriteToDestroy = spriteRenderer.gameObject;
                    spriteRenderers.Remove(spriteRenderer);

                    Spawn(newPosition);
                }
            }
        }

        return spriteToDestroy;
    }

    /// <summary>
    /// Move the sprite renderers.
    /// </summary>
    /// <param name="parallaxManager">The parallax.</param>
    /// <param name="camera">The main camera.</param>
    /// <param name="time">Time.deltaTime multiplied by the global parallax scroll speed.</param>
    public void Update(Camera camera, float time)
    {
        foreach (Renderer spriteRenderer in spriteRenderers.ToArray())
        {
            spriteRenderer.transform.Translate(time * -speedRatio, 0.0f, 0.0f);
        }
    }

    public Renderer GetSpriteRendererAtIndex(int index)
    {
        return spriteRenderers[index];
    }

    public bool UpdateDistance(Renderer spriteRenderer)
    {
        int roundedMaxWidthOfSprite = (int)Mathf.Round(widthOfSprite);
        int currentPosition = (int)Mathf.Round(spriteRenderer.transform.position.x);
        currentPosition *= -1;

        if (currentPosition - previousSpritePosition == 1)
        {
            previousSpritePosition++;

            return true;
        }
        else if (previousSpritePosition >= roundedMaxWidthOfSprite)
        {
            previousSpritePosition = 0f;
        }

        return false;
    }
}
