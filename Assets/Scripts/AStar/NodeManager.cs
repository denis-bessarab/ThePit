using UnityEngine;
using System.Collections.Generic;

public class NodeManager : Singleton<NodeManager>
{
    public static Dictionary<Vector2, Node> nodes = new();
    public static bool Ready;

    private void Start()
    {
        var nodesList = FindAllNodes();
        RegisterAvailableNodes(nodesList);
        FindNeighbourNodesForAllNodes(nodesList);
        Ready = true;
    }

    private List<Node> FindAllNodes()
    {
        var nodesList = new List<Node>();
        var nodesTilemap = GameObject.Find("Nodes");
        if (nodesTilemap == null) { Debug.LogWarning("Nodes tilemap is null"); return null;}
        for (int i = 0; i < nodesTilemap.transform.childCount; i++)
        {
            if(nodesTilemap.transform.GetChild(i).TryGetComponent(out Node node))
            {
                nodesList.Add(node);
            }
        }
        return nodesList;
    }

    private void RegisterAvailableNodes(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var go = nodes[i].gameObject;
            var pos = go.transform.position;
            RegisterNode(pos, node);
        }
    }

    private void RegisterNode(Vector2 pos, Node node)
    {
        //Debug.Log($"Registrating node {pos}");
        nodes.Add(pos, node);
    }

    private void FindNeighbourNodesForAllNodes(List<Node> nodes)
    {
        for(int i = 0; i < nodes.Count;i++)
        {
            nodes[i].FindNeighbourNodes();
        }
    }
}
