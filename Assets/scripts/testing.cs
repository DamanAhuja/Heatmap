using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UIElements;

public class testing : MonoBehaviour
{
    private grid grid;
    private void Start() {
        grid = new grid(5, 5, 10f, new Vector3(0, 0, 0));
        Debug.Log(grid);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Mouse Clicked");
            Debug.Log(UtilsClass.GetMouseWorldPosition() + "");
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            int value = grid.GetValue(position);
            grid.SetValue(position,value + 5);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Clicked");
            //Debug.Log(UtilsClass.GetMouseWorldPosition());
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}

