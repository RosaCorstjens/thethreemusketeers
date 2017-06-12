using System.Collections.Generic;
using UnityEngine;

public class GrammarGrid : Grid
{
    private List<Coordinate> hooks;
    private List<Coordinate> roomTiles;

    public GrammarGrid(int width, int height) : base(width, height)
    {
        /*char[,] test = new char[4, 1];
        test[0, 0] = 'b';
        test[1, 0] = 'o';
        test[2, 0] = 'o';
        test[3, 0] = 'b';

        SetTiles(new Coord(1, 2), test);
        SetTiles(new Coord(1, 3), test);
        SetTiles(new Coord(1, 4), test);
        SetTiles(new Coord(1, 5), test);

        char[,] test2 = new char[4, 4];
        test2[0, 0] = 'b';
        test2[1, 0] = 'o';
        test2[2, 0] = 'o';
        test2[3, 0] = 'b';
        test2[0, 1] = 'b';
        test2[1, 1] = 'o';
        test2[2, 1] = 'o';
        test2[3, 1] = 'b';
        test2[0, 2] = 'b';
        test2[1, 2] = 'o';
        test2[2, 2] = 'o';
        test2[3, 2] = 'b';
        test2[0, 3] = 'b';
        test2[1, 3] = 'o';
        test2[2, 3] = 'o';
        test2[3, 3] = 'b';

        List<Coord> coords = Contains(CreateGrid(test2));*/

       // Debug.Log(coords.Count);
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

        // can this subgrid be contained in this grid?
        if (subgrid.Width > width || subgrid.Height > height)
        {
            Debug.Log("This subgrid is too big to be contained within this grid");
            return returnList;
        }

        // loop through complete grid to find all matching subgrids
        for (int x = 0; x < width - subgrid.Width + 1; x++)
        {
            for (int y = 0; y < height - subgrid.Height + 1; y++)
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
