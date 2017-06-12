using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRuleProxy
{
    private int id;
    private List<TileGrammarRule> rules;
    private List<Coord> hooks;
    private Node node;

    public int ID { get { return id; } }
    public Node MyNode { get { return node; } }
    public int AmountOfRules { get { return rules.Count; } }

    public RoomRuleProxy(Node node)
    {
        id = node.ID;
        this.node = node;
        rules = new List<TileGrammarRule>();
        hooks = new List<Coord>();
    }

    public void AddRule(TileGrammarRule rule)
    {
        rules.Add(rule);
    }

    public TileGrammarRule GetRule(int i)
    {
        if (i < AmountOfRules && i >= 0)
        {
            return rules[i];
        }
        else
        {
            Debug.Log("Rule not found!");
            return null;
        }
    }
}

public struct Coord
{
    public int x;
    public int y;

    public Coord(int ix, int iy)
    {
        x = ix;
        y = iy;
    }
}
