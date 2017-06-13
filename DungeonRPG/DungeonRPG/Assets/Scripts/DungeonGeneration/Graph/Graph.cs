using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Graph : MonoBehaviour
{
    Node startNode;

    public Graph()
    {
        string input = File.ReadAllText("Assets/StreamingAssets/dungeonmission5.xpr");

        input = input.Replace("GRAPH", "");

        string[] splitUpInput = input.Split(')');

        Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        Dictionary<int, string> connections = new Dictionary<int, string>();

        for (int i = 0; i < splitUpInput.Length; i++)
        {
            string[] parts = splitUpInput[i].Trim().Split(':');

            if(parts[0] != "")
            {
                if (parts[1].Contains("edge"))
                {
                    connections.Add(int.Parse(parts[0]), parts[1]);
                }
                else
                {
                    string terminalSymbol = parts[1].Split('(')[0];
                    nodes.Add(int.Parse(parts[0]), new Node(terminalSymbol, int.Parse(parts[0])));
                }
            }
        }

        foreach (KeyValuePair<int, string> connection in connections)
        {
            string connectionType = connection.Value.Split('(')[0];

            string[] values = connection.Value.Split('(')[1].Split(',');
            int parentId = int.Parse(values[0].Trim());
            int childId = int.Parse(values[1].Trim());

            if(connectionType == "edge")
            {
                nodes[parentId].AddNode(nodes[childId]);
            }
            else
            {
                nodes[parentId].AddDirectedNode(nodes[childId]);
            }
        }

        startNode = nodes[0];
        PrintNodeList();
    }

    public Node StartNode
    {
        get
        {
            return startNode;
        }
    }

    public void DebugGraph()
    {
        string output = startNode.Debug();

        Debug.Log(output);
    }

    public void PrintNodeList()
    {
        List<Node> nodes = GetNodeList();
        foreach (Node node in nodes)
        {
            Debug.Log(node.Symbol);
        }
    }

    public List<Node> GetNodeList()
    {
        //ToDo:
        //startNode set according to read in file

        List<Node> nodes = new List<Node>();

        List<Node> unFinished = new List<Node>();
        unFinished.Insert(0, startNode);

        while(unFinished.Count > 0)
        {
            //first node from the unfinished list
            Node currentNode = unFinished[0];
            bool previousNotIterated = false;
            for (int i = 0; i < currentNode.Previous.Count; i++)
            {
                if (!currentNode.Previous[i].Iterated)
                {
                    unFinished.Remove(currentNode);
                    unFinished.Add(currentNode);
                    previousNotIterated = true;
                    continue;
                }
            }
            if (previousNotIterated) continue;
            if (!currentNode.Iterated)
            {
                nodes.Add(currentNode);
                currentNode.Iterated = true;
            }
            //get a child
            Connection connection = new Connection();

            int remainingConnections = currentNode.Connections.Count;
            for (int i = 0; i < currentNode.Connections.Count; i++)
            {
                if (currentNode.Connections[i].Next.Iterated)
                {
                    remainingConnections--;
                }
                else
                {
                    connection = currentNode.Connections[i];
                }
            }
            if(remainingConnections > 0)
            {
                if (connection.Next.Previous.Count <= 1)
                {
                    //inserts next node to the fron of the unfinished list
                    unFinished.Insert(0, connection.Next);
                }
                else
                {
                    if (!unFinished.Contains(connection.Next))
                    {
                        unFinished.Add(connection.Next);
                    }
                }
                if (remainingConnections == 1)
                {
                    unFinished.Remove(currentNode);
                }
            }
            else
            {
                //no other connections left remove this node
                unFinished.Remove(currentNode);
            }
        }

        foreach (var node in nodes)
        {
            node.Iterated = false;
        }


        return nodes;
    }
}
