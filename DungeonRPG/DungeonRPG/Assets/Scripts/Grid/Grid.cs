using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grid
{
    private int width, height, cellSize;
    private Vector3 origin;
    private Cell[,] cells;

    public Grid(Vector3 origin, int width, int height)
    {
        this.origin = origin;

        cellSize = 5;

        this.width = Mathf.RoundToInt((float)width/cellSize);
        this.height = Mathf.RoundToInt((float)height/cellSize);
        cells = new Cell[this.width, this.height];
    }

    public void BuildGrid()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                cells[i, j] = new Cell(i, j);
            }
        }

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                cells[i, j].FindNeighbors();
            }
        }
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return origin + new Vector3(x * cellSize, 0, y * cellSize);
    }

    public Vector2 WorldToGrid(Vector3 pos)
    {
        pos -= origin;
        return new Vector2((int)(pos.x / cellSize), (int)(pos.z / cellSize));
    }

    public Cell GetCellAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;

        return cells[x, y];
    }

    public Cell GetCellAt(Vector3 worldPosition)
    {
        Vector2 gridPosition = WorldToGrid(worldPosition);

        if (gridPosition.x < 0 || gridPosition.x >= width || gridPosition.y < 0 || gridPosition.y >= height) return null;

        return cells[Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y)];
    }

    public List<WorldObject> GetObjectsAt(Cell cell)
    {
        return cells[cell.X, cell.Y].Objects;
    }

    public List<WorldObject> GetObjectsAt(List<Cell> cells)
    {
        List<WorldObject> returnList = new List<WorldObject>();

        for (int i = 0; i < cells.Count; i++)
        {
            returnList.AddRange(cells[i].Objects);
        }

        return returnList;
    }

    public void AddObjectToGrid(WorldObject obj)
    {
        Vector2 gridPos = WorldToGrid(obj.Pos);
  
        cells[(int)gridPos.x, (int)gridPos.y].AddObject(obj);
    }

    public void UpdateObjectInGrid(WorldObject obj)
    {
        Vector2 oldCell = WorldToGrid(obj.PreviousPos);
        Vector2 newCell = WorldToGrid(obj.Pos);

        cells[(int)oldCell.x, (int)oldCell.y].RemoveObject(obj);
        cells[(int)newCell.x, (int)newCell.y].AddObject(obj);
    }

    public void RemoveObjectFromGrid(WorldObject obj)
    {
        Vector2 currentCell = WorldToGrid(obj.Pos);

        if (currentCell.x < 0 || currentCell.x >= width || currentCell.y < 0 || currentCell.y >= height) return;

        cells[(int)currentCell.x, (int)currentCell.y].RemoveObject(obj);
    }
}
