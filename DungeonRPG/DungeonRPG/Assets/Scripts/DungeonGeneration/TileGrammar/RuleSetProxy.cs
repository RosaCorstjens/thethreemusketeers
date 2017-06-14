using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSetProxy
{
    private int id;
    private List<TileGrammarRule> rules;
    private List<Coordinate> hooks;
    private Node node;

    public int ID { get { return id; } }
    public Node MyNode { get { return node; } }
    public int AmountOfRules { get { return rules.Count; } }

    public RuleSetProxy(Node node)
    {
        if (node == null)
        {
            id = -1;
            this.node = null;
        }
        else
        {
            id = node.ID;
            this.node = node;
        }
        
        rules = new List<TileGrammarRule>();
        hooks = new List<Coordinate>();
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

    public void SetDirected()
    {
        
    }
}