using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrammarRuleProxy {
    public string ruleName;
    public int width;
    public int height;

    public int maxExecutions;

    public bool canRotate;
    public bool canMirrorH;
    public bool canMirrorV;
    public bool executeRule;
    public float probability;

    private string[,] grid;

    public TileGrammarRuleProxy(string name)
    {
        ruleName = name;
        if (probability <= 0.0f)
        {
            probability = 1.0f;
        }
    }

    public void Setup()
    {
        grid = new string[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = "undefined";
            }
        }
    }

    public void SetTile(int x, int y, string value)
    {
        if (x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
        }
        else
        {
            grid[x, y] = value;
        }
    }

    public string GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
            return "Error!";
        }
        else
        {
            return grid[x, y];
        }
    }
}
