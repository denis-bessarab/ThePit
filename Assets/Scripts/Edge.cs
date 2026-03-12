using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public KeyValuePair<Vector2, Node> target;
    public EdgeType edgeType;


    public Edge(KeyValuePair<Vector2, Node> target, EdgeType edgeType)
    {
        this.target = target;
        this.edgeType = edgeType;
    }
}
