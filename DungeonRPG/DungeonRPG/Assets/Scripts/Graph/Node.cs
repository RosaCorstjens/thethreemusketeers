using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Connection> previous;
    private List<Connection> connections;
    string symbol;

    public Node(string symbol)
    {
        this.symbol = symbol;
    }

    public string Symbol
    {
        get
        {
            return Symbol;
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
        Connection newConnection = new Connection(newPrevious, false);
        previous.Add(newConnection);
    }

    public void AddDirectedNode(Node newChildNode)
    {
        Connection newConnection = new Connection(newChildNode, true);
        connections.Add(newConnection);
        newChildNode.AddPreviousNode(this);
    }

    public void AddDirectedPreviousNode(Node newPrevious)
    {
        Connection newConnection = new Connection(newPrevious, true);
        previous.Add(newConnection);
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
