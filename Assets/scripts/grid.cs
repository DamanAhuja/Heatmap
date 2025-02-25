using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class grid 
{
    public const int Max= 100;
    public const int Min= 0;
    private int width;
    private int height;
    private int [,] gridArray;
    private float cellsize;
    private Vector3 gridOrigin;
    private TextMesh[,] debugTextArray;
    
    public grid(int width,int height, float cellsize, Vector3 gridOrigin) {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;
        this.gridOrigin = gridOrigin;

        gridArray = new int[width,height];
        debugTextArray = new TextMesh[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y< gridArray.GetLength(1); y++) {
                debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y].ToString(),null, GetWorldPosition(x,y) + new Vector3 (cellsize, cellsize) * .5f,30,Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition (x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition (x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 108f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 108f);

        SetValue(2,1,56);
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) 
    {
        x = Mathf.FloorToInt((worldPosition - gridOrigin).x / cellsize);
        y = Mathf.FloorToInt((worldPosition - gridOrigin).y / cellsize);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x,y) * cellsize + gridOrigin;
    }
    public void SetValue(int x, int y, int value) 
    {

        if (x >= 0 && y >= 0 && x < width && y < height) 
        {
            gridArray[x,y] = Mathf.Clamp(value,Min,Max);
            debugTextArray[x,y].text = gridArray[x,y].ToString();
        }
    }
    public void SetValue(Vector3 worldPosition, int value) 
    {
        Debug.Log(worldPosition + "test");
        int x, y;
        GetXY(worldPosition, out x, out y);
        Debug.Log(x + "position");
        Debug.Log(y + "position");
        SetValue(x,y,value); 
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

}
