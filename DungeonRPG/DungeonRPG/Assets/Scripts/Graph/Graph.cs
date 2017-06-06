using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    Node startNode;

    public Graph()
    {
        startNode = new Node("e");
        Node node2 = new Node("n");
        Node node3 = new Node("n");
        Node node4 = new Node("t");
        Node node5 = new Node("i");
        Node node6 = new Node("ts");
        Node node7 = new Node("i");
        Node node8 = new Node("t");
        Node node9 = new Node("k");
        Node node10 = new Node("l");
        Node node11 = new Node("t");
        Node node12 = new Node("i");
        Node node13 = new Node("km");
        Node node14 = new Node("lm");
        Node node15 = new Node("ts");
        Node node16 = new Node("h"); //hub
        Node node17 = new Node("ts");
        Node node18 = new Node("km");
        Node node19 = new Node("ts");
        Node node20 = new Node("t");
        Node node21 = new Node("ts");
        Node node22 = new Node("");
        Node node23 = new Node("");
        Node node24 = new Node("");
        Node node25 = new Node("");
        Node node26 = new Node("");

        startNode.AddNode(node2);
        node2.AddNode(node3);

        node3.AddNode(node4);
        node4.AddNode(node5);

        node3.AddNode(node6);
        node6.AddNode(node7);

        node3.AddNode(node8);
        node8.AddNode(node9);
        node9.AddNode(node10);

        node10.AddNode(node11);
        node11.AddNode(node12);

        node10.AddNode(node13);
        node13.AddNode(node14);
        node14.AddNode(node15);
        node15.AddNode(node16);

        node3.AddNode(node17);
        node17.AddNode(node18);
        node18.AddNode(node14);

        node2.AddNode(node19);
        node19.AddNode(node20);
        node20.AddNode(node21);
        node21.AddNode(node15);


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
        List<string> nodes = GetNodeList();
        foreach (string node in nodes)
        {
            Debug.Log(node);
        }
    }

    public List<string> GetNodeList()
    {
        //ToDo:
        //startNode set according to read in file

        List<string> nodes = new List<string>();

        List<Node> unFinished = new List<Node>();
        unFinished.Insert(0, startNode);

        while(unFinished.Count > 0)
        {
            //first node from the unfinished list
            Node currentNode = unFinished[0];
            if (!currentNode.Iterated)
            {
                nodes.Add(currentNode.Symbol);
                currentNode.Iterated = true;
            }
            if (currentNode.Connections.Count > 0)
            {
                //get a child
                Connection connection = currentNode.Connections[0];
                //remove child from connections
                currentNode.Connections.RemoveAt(0);
                if (connection.Next.Previous.Count <= 1)
                {
                    //adds the node its symbol to the nodes list
                    if (connection.IsLiniar)
                    {
                        //adds connected command ot the string list 
                        nodes.Add("connected");
                    }
                    //inserts next node to the fron of the unfinished list
                    unFinished.Insert(0, connection.Next);
                }
                else
                {
                    bool temp = true;
                    for (int i = 0; i < connection.Next.Previous.Count; i++)
                    {
                        if (!connection.Next.Previous[i].Iterated)
                        {
                            temp = false;
                        }
                    }
                    //node with multiple incoming connections
                    if (temp)
                    {
                        //adds it to the end of the unfinished list when it is not already in it
                        unFinished.Insert(0, connection.Next);
                    }
                }
                if(currentNode.Connections.Count == 0)
                {
                    //no other connections left remove this node
                    unFinished.Remove(currentNode);
                }
            }
            else
            {
                unFinished.Remove(currentNode);
            }
        }


        return nodes;
    }
}
