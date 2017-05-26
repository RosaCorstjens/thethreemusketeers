using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    private const char UNDEFINED = 'u';
    private char[,] tiles;
    private int width, height;

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

        char[,] test = new char[4,1];
        test[0, 0] = 'B';
        test[1, 0] = 'O';
        test[2, 0] = 'O';
        test[3, 0] = 'B';

        SetTiles(new Coordinate(2, 2), test);
    }

    public char GetTile(Coordinate position)
    {
        return tiles[position.x, position.y];
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

    public void printGrid(Text output)
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