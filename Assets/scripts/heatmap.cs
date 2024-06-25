using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heatmap : MonoBehaviour
{
    public Renderer serverRenderer;
    public List<Server> servers = new List<Server>(); // List of servers
    private Texture2D heatmapTexture;
    private float[,] heatValues;
    private int textureWidth = 100;
    private int textureHeight = 100;

    public bool IsInitialized { get; private set; } = false;

    void Start()
    {
        // Initialize the server renderer
        serverRenderer = GetComponent<Renderer>();

        if (serverRenderer == null)
        {
            Debug.LogError("Renderer component is missing!");
            return;
        }

        // Check if the material and shader are correct
        if (serverRenderer.material == null || !serverRenderer.material.HasProperty("_HeatMap"))
        {
            Debug.LogError("Material is missing or does not have a '_HeatMap' property!");
            return;
        }

        // Create and assign the heatmap texture
        heatmapTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RFloat, false);
        serverRenderer.material.SetTexture("_HeatMap", heatmapTexture);

        // Initialize heat values array
        heatValues = new float[textureWidth, textureHeight];

        // Mark initialization complete
        IsInitialized = true;

        // Start fetching heat data
        StartCoroutine(FetchHeatData());
    }
    /*private void Update()
    {
        UpdateHeatmapTexture();
    }*/
    public void RegisterServer(Server server)
    {
        if (!servers.Contains(server))
        {
            servers.Add(server);
        }
    }

    IEnumerator FetchHeatData()
    {
        while (true)
        {
            // Clear heat values
            System.Array.Clear(heatValues, 0, heatValues.Length);

            // Update the heat values based on each server's data
            foreach (Server server in servers)
            {
                Vector2 position = server.GetHeatmapPosition(textureWidth, textureHeight);
                float heatValue = server.heatOutput;
                int x = (int)position.x;
                int y = (int)position.y;
                for (int i = x - 3; i < x + 3; i++)
                {
                    for (int j = y - 3; j < y + 3; j++)
                    {
                        if (position.x >= 0 && position.x < textureWidth && position.y >= 0 && position.y < textureHeight)
                        {
                            heatValues[i, j] = heatValue;
                            //Debug.Log(i + j);
                        }
                    }
                }
            }
            // Update the heatmap texture
            UpdateHeatmapTexture();

            // Wait for a second before fetching new data
            yield return new WaitForSeconds(1);
        }
    }

    void UpdateHeatmapTexture()
    {
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                //Debug.Log(heatValues[x, y]);
                float heat = heatValues[x, y] / 100.0f; // Normalize heat value to 0-1 range
                heatmapTexture.SetPixel(x, y, new Color(heat, 0, 0, 1)); // Store normalized heat value in the red channel
            }
        }

        heatmapTexture.Apply();
    }
}
