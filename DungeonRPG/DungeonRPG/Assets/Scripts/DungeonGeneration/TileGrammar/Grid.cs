using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Orientation
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public class Grid
{
    protected const char UNDEFINED = 'u';
    protected char[,] tiles;
    protected int width, height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }


    public Grid(int width, int height)
    {
        tiles = new char[width, height];
        this.width = width;
        this.height = height;

        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                SetTile(new Coordinate(ix, iy), UNDEFINED);
            }
        }
    }

    public Grid(char[,] grid)
    {
        width = grid.GetLength(0);
        height = grid.GetLength(1);
        tiles = new char[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles[x, y] = grid[x, y];
            }
        }
    }

    public char GetTile(Coordinate position)
    {
        return tiles[position.x, position.y];
    }

    public char GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public void SetTile(Coordinate position, char value)
    {
        tiles[position.x, position.y] = value;
    }

    Grid GetTiles(Coordinate position, Coordinate size)
    {
        Grid returnGrid = new Grid(size.x, size.y);
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                returnGrid.SetTile(new Coordinate(ix, iy), tiles[position.x + ix, position.y + iy]);
            }
        }
        return returnGrid;
    }

    public void SetTiles(Coordinate position, Grid value)
    {
        for (int ix = 0; ix < value.width; ix++)
        {
            for (int iy = 0; iy < value.height; iy++)
            {
                SetTile(new Coordinate(position.x + ix, position.y + iy), value.GetTile(new Coordinate(ix, iy)));
            }
        }
    }

    public void SetTiles(Coordinate position, char[,] value)
    {
        for (int ix = 0; ix < value.GetLength(0); ix++)
        {
            for (int iy = 0; iy < value.GetLength(1); iy++)
            {
                SetTile(new Coordinate(position.x + ix, position.y + iy), value[ix, iy]);
            }
        }
    }

    public void SetTiles(char[,] value)
    {
        for (int ix = 0; ix < value.GetLength(0); ix++)
        {
            for (int iy = 0; iy < value.GetLength(1); iy++)
            {
                SetTile(new Coordinate(ix, iy), value[ix, iy]);
            }
        }
    }

    public void PrintGrid(Text output)
    {
        string outText = "";
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                outText += "|" + ((tiles[iy, ix] == 'u')? "." : "0") + "|";
            }
            outText += "\n";
        }
        output.text = outText;
    }

    public bool Equals(Coordinate startPos, Grid matchGrid)
    {
        for (int x = startPos.x; x < matchGrid.width + startPos.x; x++)
        {
            for (int y = startPos.y; y < matchGrid.height + startPos.y; y++)
            {
                if (GetTile(x,y) != matchGrid.GetTile(x - startPos.x, y - startPos.y))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static Grid CreateGrid(char[,] gridContent)
    {
        Grid returnGrid = new Grid(gridContent.GetLength(0), gridContent.GetLength(1));

        returnGrid.SetTiles(gridContent);

        return returnGrid;
    }

    public static Grid RotateGrid(Grid grid, Orientation newOrientation)
    {
        switch (newOrientation)
        {
            case Orientation.North:
                return grid;
            case Orientation.East:
                grid = RotateGrid90Degrees(grid);
                break;
            case Orientation.South:
                grid = RotateGrid90Degrees(RotateGrid90Degrees(grid));
                break;
            case Orientation.West:
                grid = RotateGrid90Degrees(RotateGrid90Degrees(RotateGrid90Degrees(grid)));
                break;
            default:
                return grid;
        }

        return grid;
    }

    // https://stackoverflow.com/questions/18034805/rotate-mn-matrix-90-degrees
    private static Grid RotateGrid90Degrees(Grid grid)
    {
        Grid returnGrid = new Grid(grid.height, grid.width);

        int newCol, newRow = 0;

        for (int oldCol = grid.Height - 1; oldCol >= 0; oldCol--)
        {
            newCol = 0;

            for (int oldRow = 0; oldRow < grid.Width; oldRow++)
            {
                returnGrid.SetTile(new Coordinate(newRow, newCol), grid.GetTile(oldRow, oldCol));
                newCol++;
            }
            newRow++;
        }

        return returnGrid;
    }
}

public struct Coordinate
{
    public int x;
    public int y;
    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}