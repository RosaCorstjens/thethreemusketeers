using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCreator
{
    private TileRuleParser parser;
    private Graph graph;
    private List<Node> graphRecipe;
    private List<TileGrammarRule> tileRecipe;
    private List<RoomRuleProxy> roomList;
    private int monsterPerRoom = 3;
    private int trapsPerRoom = 3;

    public static Dictionary<string, List<string>> graphToTile;

    public RecipeCreator()
    {
        parser = new TileRuleParser();
        graph = new Graph();

        graphToTile = new Dictionary<string, List<string>>();
        GetGraphToTileTranslations();

        graphRecipe = graph.GetNodeList();
    }

    public List<RoomRuleProxy> getRoomList()
    {
        roomList = new List<RoomRuleProxy>(graphRecipe.Count);
        for (int i = 0; i < graphRecipe.Count; i++)
        {
            roomList.Add(ReplaceRule(graphRecipe[i]));
        }
        return roomList;
    }

    RoomRuleProxy ReplaceRule(Node node)
    {
        RoomRuleProxy room = new RoomRuleProxy(node);
        List<string> ruleNames = graphToTile[node.Symbol];
        for (int i = 0; i < ruleNames.Count; i++)
        {
            room.AddRule(parser.getRule(ruleNames[i]));
        }

        return room;
    }

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
        content.Add("finalize");
        graphToTile.Add("hub", new List<string>(content));
    }
}
