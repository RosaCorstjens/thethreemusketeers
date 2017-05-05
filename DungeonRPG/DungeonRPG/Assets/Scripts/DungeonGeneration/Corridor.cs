using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Corridor
{
    private List<Floor> floors;
    public List<Floor> Floors { get { return floors; } set { floors = value; } }

    public void BuildCorridor()
    {
        floors.HandleAction(f => f.PlaceWalls());
    }

    public void Destroy()
    {
        floors.HandleAction(f => f.Destroy());
        floors.Clear();
    }
}
