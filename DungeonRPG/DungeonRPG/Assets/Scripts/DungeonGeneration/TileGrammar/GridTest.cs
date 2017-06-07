using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTest : MonoBehaviour
{
    private GrammarGrid grid;
    private TileGrammarHandler handler;
    private int i = 0;

    public Text txt;

	// Use this for initialization
	void Start ()
	{
	    grid = new GrammarGrid(10, 10);
        handler = new TileGrammarHandler("");
	    handler.ApplyRecipe(grid);
	}

    public void Print()
    {
        switch (i)
        {
            case 0:
                grid.PrintGrid(txt);
                break;
            case 1:
                Grid gridEast = Grid.RotateGrid(grid, Orientation.East);
                Debug.Log("East");
                gridEast.PrintGrid(txt);
                break;
            case 2:
                Grid gridSouth = Grid.RotateGrid(grid, Orientation.South);
                Debug.Log("South");
                gridSouth.PrintGrid(txt);
                break;
            case 3:
                Grid gridWest = Grid.RotateGrid(grid, Orientation.West);
                Debug.Log("West");
                gridWest.PrintGrid(txt);
                break;
            case 4:
                Grid gridNorth = Grid.RotateGrid(grid, Orientation.North);
                Debug.Log("North");
                gridNorth.PrintGrid(txt);
                break;
        }
        i++;

    }
}
