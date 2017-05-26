using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    protected const char UNDEFINED = 'u';
    protected char[,] tiles;
    protected int width, height;

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
                outText += "|" + tiles[iy, ix] + "|";
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