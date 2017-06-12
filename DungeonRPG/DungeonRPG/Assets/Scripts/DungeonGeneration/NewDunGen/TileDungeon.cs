using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDungeon
{
    private TileDungeonManager dm; // ref to dungeon manager

    private char[][] tiles;
    private List<Floor> floors;

    private List<EnemyController> enemies;

    public List<EnemyController> Enemies
    {
        get { return enemies; }
    }

    private List<LootChest> lootchests;
    //TODO: define other content

    private Vector3 startPosition;

    public Vector3 StartPosition
    {
        get { return startPosition; }
    }

    private int level;

    public Floor GetFloor(int xPos, int yPos)
    {
        return floors.Find(f => f.xPos == xPos && f.yPos == yPos);
    }

    public void Initialize(char[][] tiles)
    {
        dm = GameManager.Instance.DungeonManager;
        this.tiles = tiles;

        // instantiates all floor tiles
        InstantiateFloors();

        // spawns content in the dungeon
        SpawnContent();
    }

    private void InstantiateFloors()
    {
        floors = new List<Floor>();

        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // if the tile is undefined
                if (tiles[i][j] == 'u') continue;

                // create a new floor object
                Floor newFloor = new Floor();
                newFloor.Initialize(i, j);
                floors.Add(newFloor);
            }
        }

        floors.HandleAction(f => f.DetermineNeighbours());
        floors.HandleAction(f => f.PlaceFloor());
        floors.HandleAction(f => f.PlaceWalls());
    }

    private void SpawnContent()
    {
    }

    public void ClearDungeon()
    {
    }

    public void RestartDungeon()
    {
    }

    public bool IsOccupied(int xPos, int yPos)
    {
        if (tiles[xPos][yPos] == 'u') return false;

        return true;
    }
}

