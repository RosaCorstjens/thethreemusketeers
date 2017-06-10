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
    public List<float> ProbabilitiesRHS { get; set; }

    private char[,] lhs;
    private List<char[,]> rhs;

    public TileGrammarRuleProxy(string name)
    {
        ruleName = name;
        rhs = new List<char[,]>();
        ProbabilitiesRHS = new List<float>();
    }

    public void SetupLHS()
    {
        lhs = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                lhs[x, y] = 'u';
            }
        }
    }

    public void SetupRHS()
    {
        rhs.Add(new char[width,height]);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                rhs[rhs.Count - 1][x, y] = 'u';
            }
        }
    }

    public void SetLHSTile(int x, int y, char value)
    {
        if (x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
        }
        else
        {
            lhs[x, y] = value;
        }
    }

    public char GetLHSTile(int x, int y)
    {
        if (x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
            return '!';
        }
        else
        {
            return lhs[x, y];
        }
    }

    public void SetRHSTile(int id, int x, int y, char value)
    {
        if (id > rhs.Count || x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
        }
        else
        {
            rhs[id][x, y] = value;
        }
    }

    public void SetRHS(int id, char[,] valueGrid)
    {
        if (valueGrid.GetLength(0) >= width && valueGrid.GetLength(1) >= height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rhs[id][x, y] = valueGrid[x, y];
                }
            }
        }
        else
        {
            Debug.LogError("Grid not big enough to save in this rule: " + ruleName);
        }
    }

    public char GetRHSTile(int id, int x, int y)
    {
        if (id > rhs.Count || x < 0 || y < 0 || x > width || y > height)
        {
            Debug.Log("gridposition out of bound.");
            return '!';
        }
        else
        {
            return rhs[id][x, y];
        }
    }

    public TileGrammarRule getRule()
    {
        Grid LHS = new Grid(lhs);
        List<Grid> RHS = new List<Grid>();
        List<float> probs = new List<float>();
        for (int i = 0; i < rhs.Count; i++)
        {
            RHS.Add(new Grid(rhs[i]));
            if (ProbabilitiesRHS.Count > i)
            {
                probs.Add(ProbabilitiesRHS[i]);
            }
            else
            {
                probs.Add(1.0f);
            }
        }

        return new TileGrammarRule(ruleName, LHS, RHS, probs);

    }
}
