using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTest : MonoBehaviour
{
    private Grid grid;
    public Text txt;

	// Use this for initialization
	void Start ()
	{
	    grid = new Grid(10, 10);

	}

    public void Print()
    {
        grid.printGrid(txt);
    }
}
