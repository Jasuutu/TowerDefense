using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid<TGridObject> 
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray;
    private Vector3 originPosition;

    #region Events
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x, y;
    }
    #endregion

    public CustomGrid(int width, int height, float cellSize, Vector3 originPosition, Func<CustomGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for(int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = createGridObject(this, i, j);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = CreateText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 10, Color.green, 0.25f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (o, e) =>
            {
                debugTextArray[e.x, e.y].text = gridArray[e.x, e.y]?.ToString();
            };
        }
    }

    #region Helpers

    public int GetWidth() { return width; }

    public int GetHeight() { return height; }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public TGridObject GetGridObect(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default;
        }
    }

    public TGridObject GetGridObect(Vector3 worldPosition)
    {
        int x, y;

        GetXY(worldPosition, out x, out y);
        return GetGridObect(x, y);
    }


    public void SetGridObject(int x, int y , TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;            
        }

        TriggerGridObjectChanged(x,y);
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;

        GetXY(worldPosition, out x,out y);
        SetGridObject(x, y, value);
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private TextMesh CreateText(string text, Transform parent = null, Vector3 locatPostion = default, 
                                int fontSize = 40, Color color = default, float characterSize = 1f, TextAnchor textAnchor = TextAnchor.MiddleCenter, 
                                TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 1)
    {
        GameObject gameObject = new GameObject("RTText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = locatPostion;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.characterSize = characterSize;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    #endregion
}
