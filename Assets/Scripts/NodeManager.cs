using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodes = new List<Node>();

    public void RegisterNode(Node node)
    {
        nodes.Add(node);
    }
}
