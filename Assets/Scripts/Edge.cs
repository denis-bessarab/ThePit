using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public KeyValuePair<Vector2, Node> targetNode;
    public EdgeType edgeType;


    public Edge(KeyValuePair<Vector2, Node> targetNode, EdgeType edgeType)
    {
        this.targetNode = targetNode;
        this.edgeType = edgeType;
    }
}
