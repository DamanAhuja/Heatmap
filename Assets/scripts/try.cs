using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Try : MonoBehaviour
{
    public Renderer serverRenderer;
    public List<Server> servers = new List<Server>(); // List of servers
    private Texture2D heatmapTexture;
    private float[,] heatValues;
    private int textureWidth = 100;
    private int textureHeight = 100;

    public bool IsInitialized { get; private set; } = false;
    public int TextureWidth { get { return textureWidth; } }
    public int TextureHeight { get { return textureHeight; } }

    void Start()
    {
        // Initialize the server renderer
        serverRenderer = GetComponent<Renderer>();

        if (serverRenderer == null)
        {
            Debug.LogError("Renderer component is missing!");
            return;
        }

        // Create and assign the heatmap texture
        heatmapTexture = new Texture2D(textureWidth, textureHeight);
        serverRenderer.material.mainTexture = heatmapTexture;

        // Initialize heat values array
        heatValues = new float[textureWidth, textureHeight];

        // Mark initialization complete
        IsInitialized = true;

        // Start a coroutine to fetch heat data after a delay
        StartCoroutine(WaitAndFetchHeatData());
    }

    public void RegisterServer(Server server)
    {
        if (!servers.Contains(server))
        {
            servers.Add(server);
            Debug.Log($"Server registered at position: {server.GetHeatmapPosition(textureWidth, textureHeight)}");
        }
    }

    private IEnumerator WaitAndFetchHeatData()
    {
        yield return new WaitForSeconds(2.0f);

        FetchHeatData();
    }

    private void FetchHeatData()
    {
        Debug.Log("FetchHeatData function called");

        // Check if servers list is empty
        if (servers.Count == 0)
        {
            Debug.LogWarning("No servers registered.");
            return;
        }

        // Clear the entire heatmap once at the beginning
        for (int i = 0; i < textureHeight; i++)
        {
            for (int j = 0; j < textureWidth; j++)
            {
                Color col;
                col = Color.blue;
                heatmapTexture.SetPixel(i, j, col);
                heatValues[i, j] = 0f;
            }
        }

        // Apply heat values from servers
        foreach (Server server in servers)
        {
            Vector2 position = server.GetHeatmapPosition(textureWidth, textureHeight);
            Debug.Log("Server position: " + position);
            float heatValue = server.heatOutput;
            int x = (int)position.x;
            int y = (int)position.y;

            if (x < 0 || x >= textureWidth || y < 0 || y >= textureHeight)
            {
                Debug.LogWarning($"Server position ({x}, {y}) is out of bounds. Skipping.");
                continue;
            }
            if (heatValue >= 80f)
            {
                ApplyHeatValue(x, y, 8, 20f);
                ApplyHeatValue(x, y, 6, 20f);
                ApplyHeatValue(x, y, 4, 20f);
                ApplyHeatValue(x, y, 2, 20f);
            }
            else if (heatValue >= 60f)
            {
                ApplyHeatValue(x, y, 6, 20f);
                ApplyHeatValue(x, y, 4, 20f);
                ApplyHeatValue(x, y, 2, 20f);
            }
            else if (heatValue >= 40f)
            {
                ApplyHeatValue(x, y, 4, 20f);
                ApplyHeatValue(x, y, 2, 20f);
            }
            else if (heatValue >= 20f)
            {
                ApplyHeatValue(x, y, 2, 20f);
            }
        }

        heatmapTexture.Apply();
        Debug.Log("Texture Applied");
    }

    private void ApplyHeatValue(int centerX, int centerY, int radius, float heatValue)
    {
        for (int i = centerX - radius; i <= centerX + radius; i++)
        {
            for (int j = centerY - radius; j <= centerY + radius; j++)
            {
                if (i >= 0 && i < textureWidth && j >= 0 && j < textureHeight)
                {
                    SetPixel(i, j, heatValue);
                }
            }
        }
    }

    private void SetPixel(int i, int j, float heatValue)
    {
        heatValues[i, j] += heatValue;

        Color color;
        if (heatValues[i, j] < 20)
        {
            color = Color.blue;
        }
        else if (heatValues[i, j] < 40)
        {
            color = new Color(0, 1, 1); // Cyan
        }
        else if (heatValues[i, j] < 60)
        {
            color = Color.green;
        }
        else if (heatValues[i, j] < 80)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.red;
        }
        heatmapTexture.SetPixel(i, j, color);
    }

    public void UpdateServerPosition()
    {
        FetchHeatData();
    }
}

