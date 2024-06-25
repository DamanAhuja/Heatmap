using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
    public float heatOutput = 100f;

    private Try heatmap;
    private Vector2 lastPosition;

    void Start()
    {
        heatmap = FindObjectOfType<Try>();
        if (heatmap != null)
        {
            StartCoroutine(RegisterServerAfterInitialization());
        }
        else
        {
            Debug.LogError("HeatmapManager not found in the scene!");
        }

        // Start the position check coroutine
        StartCoroutine(CheckPositionChange());
    }

    private IEnumerator RegisterServerAfterInitialization()
    {
        // Wait until the HeatmapManager is initialized
        while (!heatmap.IsInitialized)
        {
            yield return null;
        }

        // Register server and store initial position
        heatmap.RegisterServer(this);
        lastPosition = GetHeatmapPosition(heatmap.TextureWidth, heatmap.TextureHeight);
    }

    private IEnumerator CheckPositionChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds

            Vector2 currentPosition = GetHeatmapPosition(heatmap.TextureWidth, heatmap.TextureHeight);
            if (currentPosition != lastPosition)
            {
                lastPosition = currentPosition;
                heatmap.UpdateServerPosition();
            }
        }
    }

    public Vector2 GetHeatmapPosition(int textureWidth, int textureHeight)
    {
        // Convert the server's world position to the heatmap's texture coordinates
        Vector2 position = new Vector2((textureWidth / 2) - transform.position.x, (textureHeight / 2) - transform.position.z);
        //Debug.Log(origin);
        return position;
    }
}
