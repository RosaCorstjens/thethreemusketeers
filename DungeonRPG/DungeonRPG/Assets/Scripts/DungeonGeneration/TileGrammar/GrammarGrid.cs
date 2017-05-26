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
        char[,] test = new char[4, 1];
        test[0, 0] = 'B';
        test[1, 0] = 'O';
        test[2, 0] = 'O';
        test[3, 0] = 'B';

        SetTiles(new Coordinate(2, 2), test);
        SetTiles(new Coordinate(2, 3), test);
        SetTiles(new Coordinate(2, 4), test);
        SetTiles(new Coordinate(2, 5), test);

        char[,] test2 = new char[4, 4];
        test2[0, 0] = 'B';
        test2[1, 0] = 'O';
        test2[2, 0] = 'O';
        test2[3, 0] = 'B';
        test2[0, 1] = 'B';
        test2[1, 1] = 'O';
        test2[2, 1] = 'O';
        test2[3, 1] = 'B';
        test2[0, 2] = 'B';
        test2[1, 2] = 'O';
        test2[2, 2] = 'O';
        test2[3, 2] = 'B';
        test2[0, 3] = 'B';
        test2[1, 3] = 'O';
        test2[2, 3] = 'O';
        test2[3, 3] = 'B';

        List<Coordinate> coords = Contains(CreateGrid(test2));

        Debug.Log(coords.Count);
    }

    /// <summary>
    /// Determines whether the grid contains the given subgrid.
    /// </summary>
    /// <param name="subgrid">The subgrid.</param>
    /// <returns>
    ///   <c>true</c> if the grid contains the given subgrid; otherwise, <c>false</c>.
    /// </returns>
    public List<Coordinate> Contains(Grid subgrid)
    {
        List <Coordinate> returnList = new List<Coordinate>();

        // loop through complete grid to find all matching subgrids
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // check for [0,0]
                if (subgrid.GetTile(0, 0) == GetTile(x, y))
                {
                    Coordinate c = new Coordinate(x,y);

                    // match the subgrid with this grid from the current x, y
                    if (Equals(c, subgrid))
                    {
                        // add the x, y pos to the options
                        returnList.Add(c);
                    }
                }
            }
        }

        return returnList;
    }
}
