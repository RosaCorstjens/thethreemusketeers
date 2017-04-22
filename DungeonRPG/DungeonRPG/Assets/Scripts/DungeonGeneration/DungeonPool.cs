using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonPool
{
    Stack<Dungeon> normalDungeons;
    Stack<Dungeon> miniBossDungeons;

    public bool HasNormalDungeons { get { return normalDungeons.Count != 0; } }
    public int NumNormalDungeons { get { return normalDungeons.Count; } }

    private int maxMiniBossDungeonsStored = 3;
    public bool HasMiniBossDungeons { get { return miniBossDungeons.Count != 0; } }
    public bool CanStoreMiniBossDungeons { get { return miniBossDungeons.Count < maxMiniBossDungeonsStored; } }
    public int NumMiniBossDungeons { get { return miniBossDungeons.Count; } }

    public void Initialize()
    {
        normalDungeons = new Stack<Dungeon>();
        miniBossDungeons = new Stack<Dungeon>();
    }    

    public void AddNormalDungeon(Dungeon dungeon)
    {
        normalDungeons.Push(dungeon);
    }

    public void AddMiniBossDungeon(Dungeon dungeon)
    {
        miniBossDungeons.Push(dungeon);
    }

    public Dungeon GetNormalDungeon()
    {
        return normalDungeons.Pop();
    }

    public Dungeon GetMiniBossDungeon()
    {
        return miniBossDungeons.Pop();
    }

    public bool CanStore(Dungeon.DungeonType type)
    {
        if (type == Dungeon.DungeonType.MiniBoss)
        {
            if (CanStoreMiniBossDungeons) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }
}
