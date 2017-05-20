using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    Node startNode;

    public Graph()
    {
        startNode = new Node("S");
    }

    public Node StartNode
    {
        get
        {
            return startNode;
        }
    }

    public static void Replace(Node replace)
    {
        List<Connection> tempConnections = replace.Connections;

        switch (replace.Symbol)
        {
            case "S":
                replace.ChangeSymbol("e");
                Node part = new Node("P");
                Node chainFinal = new Node("CF");
                Node goal = new Node("g");

                goal.SetConnections(tempConnections);
                chainFinal.AddNode(goal);
                part.AddNode(chainFinal);
                replace.AddNode(part);
                break;
            case "F":
                foreach (var Connection in replace.Connections)
                {
                    if(Connection.Next.Symbol == "")
                }
                break;
            default:
                Debug.Log("non replaceable symbol");
                break;
        }
        foreach (var connection in replace.Connections)
        {
            Replace(connection.Next);
        }
    }

    public void DebugGraph()
    {
        string output = startNode.Debug();

        Debug.Log(output);
    }
}
