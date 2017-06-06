using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Node> previous;
    private List<Connection> connections;
    string symbol;
    public bool Iterated = false;

    public Node(string symbol)
    {
        this.symbol = symbol;
        previous = new List<Node>();
        connections = new List<Connection>();
    }

    public string Symbol
    {
        get
        {
            return symbol;
        }
    }
    
    public void ChangeSymbol(string newSymbol)
    {
        symbol = newSymbol;
        connections = new List<Connection>();
    }

    public void SetConnections(List<Connection> newConnections)
    {
        connections = newConnections;
    }

    public List<Connection> Connections
    {
        get
        {
            if(connections == null)
            {
                connections = new List<Connection>();
            }
            return connections;
        }
    }

    public List<Node> Previous
    {
        get
        {
            if(previous == null)
            {
                previous = new List<Node>();
            }
            return previous;
        }
    }

    public void createChildNode(string pSymbol)
    {
        Node newNode = new Node(pSymbol);
        AddNode(newNode);
    }

    public void AddNode(Node newChildNode)
    {
        Connection newConnection = new Connection(newChildNode, false);
        connections.Add(newConnection);
        newChildNode.AddPreviousNode(this);
    }

    public void AddPreviousNode(Node newPrevious)
    {
        previous.Add(newPrevious);
    }

    public void AddDirectedNode(Node newChildNode)
    {
        Connection newConnection = new Connection(newChildNode, true);
        connections.Add(newConnection);
        newChildNode.AddPreviousNode(this);
    }


    public string Debug()
    {
        string returnString = symbol + " ";

        foreach (var connection in connections)
        {
            returnString = returnString + connection.Next.Debug();
        }

        return returnString;
    }
}
