using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCreator
{
    private TileRuleParser parser;
    private Graph graph;
    private List<Node> graphRecipe;
    private List<TileGrammarRule> tileRecipe;
    private List<RuleSetProxy> roomList;
    private int monsterPerRoom = 3;
    private int trapsPerRoom = 3;

    public static Dictionary<string, List<string>> graphToTile;

    /// <summary>
    /// Initializes a new instance of the <see cref="RecipeCreator"/> class.
    /// </summary>
    public RecipeCreator()
    {
        parser = new TileRuleParser();
        graph = new Graph();

        graphToTile = new Dictionary<string, List<string>>();
        GetGraphToTileTranslations();

        graphRecipe = graph.GetNodeList();
    }

    /// <summary>
    /// Creates the recipe.
    /// </summary>
    /// <returns></returns>
    public List<RuleSetProxy> getRoomList()
    {
        roomList = new List<RuleSetProxy>(graphRecipe.Count);
        for (int i = 0; i < graphRecipe.Count; i++)
        {
            bool isConnected = false;
            if (i - 1 >= 0) isConnected = graphRecipe[i-1].Connections.FindAll(c => c.IsDirected).Count > 0;

            roomList.Add(ReplaceRule(graphRecipe[i], isConnected, graphRecipe[i].Connections.FindAll(c => c.IsDirected).Count > 0));
        }
        roomList.Add(getFinalRules());
        return roomList;
    }

    public RuleSetProxy getFinalRules()
    {
        RuleSetProxy proxy = new RuleSetProxy(null);
        proxy.AddRule(parser.GetRule("connect-hooks"));
        proxy.AddRule(parser.GetRule("removehooks"));
        return proxy;
    }

    /// <summary>
    /// Replaces a node with the matching tile rules.
    /// </summary>
    /// <param name="node">The node to replace.</param>
    /// <returns>A proxy object containing data about the rules etc.</returns>
    RuleSetProxy ReplaceRule(Node node, bool isConnected = false, bool nextIsConnected = false)
    {
        RuleSetProxy room = new RuleSetProxy(node);
        List<string> ruleNames = graphToTile[node.Symbol];
        for (int i = 0; i < ruleNames.Count; i++) {
            if (ruleNames[i] == "room" && isConnected) {
                room.AddRule(parser.GetRule("connectedRoom"));
                room.AddRule(parser.GetRule("undirectRoom"));
            }
            else
            {
                room.AddRule(parser.GetRule(ruleNames[i]));
            }

            if (ruleNames[i] == "room" && nextIsConnected) room.AddRule(parser.GetRule("directRoom"));
        }

        return room;
    }

    /// <summary>
    /// Sets the graph node to tile rules translations.
    /// </summary>
    void GetGraphToTileTranslations()
    {
        List<string> content = new List<string>();
        content.Clear();
        content.Add("room");
        content.Add("treasure");
        content.Add("finalize");
        graphToTile.Add("items", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("key");
        content.Add("finalize");
        graphToTile.Add("key", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("keyfinal");
        content.Add("finalize");
        graphToTile.Add("keyfinal", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("key-multi");
        content.Add("finalize");
        graphToTile.Add("keymulti", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("lock");
        content.Add("treasure");
        content.Add("finalize");
        graphToTile.Add("lock", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("lockfinal");
        content.Add("finalize");
        graphToTile.Add("lockfinal", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("lock-multi");
        content.Add("treasure");
        content.Add("finalize");
        graphToTile.Add("lockmulti", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("finalize");
        graphToTile.Add("none", new List<string>(content));

        content.Clear();
        content.Add("room");
        //TODO: make nmonsters random 
        for (int i = 0; i < monsterPerRoom; i++)
        {
            content.Add("monster");
        }
        content.Add("finalize");
        graphToTile.Add("test", new List<string>(content));

        content.Clear();
        content.Add("room");
        //TODO: make nTraps random 
        for (int i = 0; i < trapsPerRoom; i++)
        {
            content.Add("trap");
        }
        content.Add("finalize");
        graphToTile.Add("testsecret", new List<string>(content));

        content.Clear();
        content.Add("entrance");
        content.Add("finalize");
        graphToTile.Add("entrance", new List<string>(content));

        content.Clear();
        content.Add("room");
        content.Add("portal");
        content.Add("finalize");
        graphToTile.Add("goal", new List<string>(content));

        content.Clear();
        content.Add("hub");
        content.Add("undirectRoom");
        content.Add("directRoom");
        //evt content
        content.Add("finalize");
        content.Add("removehooks");
        graphToTile.Add("hub", new List<string>(content));
    }
}
