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

    public void DebugGraph()
    {
        string output = startNode.Debug();

        Debug.Log(output);
    }
}
