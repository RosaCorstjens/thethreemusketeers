using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GrammarGrid : Grid
{
    private List<Coordinate> hooks;
    private List<Coordinate> roomTiles;

    public GrammarGrid(int width, int height) : base(width, height)
    {
        
    }

    public bool Contains(Grid value)
    {

        return false;
    }
}
