using UnityEngine;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{
    private Queue<Node> openList = new();
    private List<Node> closedList = new();

    private float DistanceToTarget(Node currentNode,Node target)
    {
        return Vector2.Distance(currentNode.position, target.position);
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        List<Node> path = new();

        openList.Enqueue(startNode);
        path.Add(startNode);

        while (openList.Count > 0)
        {
            var currentNode = openList.Dequeue();
            if (currentNode.position == targetNode.position) return path;

            var edges = currentNode.edges;
            var nextEdge = FindEdgeWithMaxWeight(currentNode, targetNode, edges, closedList);

            closedList.Add(currentNode);

            openList.Enqueue(nextEdge.targetNode.Value);
            path.Add(nextEdge.targetNode.Value);
        }

        return path;
    }

    private Edge FindEdgeWithMaxWeight(Node currentNode, Node targetNode, List<Edge> edges, List<Node> closedList)
    {
        Edge nextEdge;
        var currentDistance = DistanceToTarget(currentNode, targetNode);
        int minIndex = -1;
        for (int i = 0; i < edges.Count; i++)
        {
            if (closedList.Contains(edges[i].targetNode.Value)) continue;

            var distanceToTarget = DistanceToTarget(edges[i].targetNode.Value, targetNode);
            if (distanceToTarget < currentDistance)
            {
                if (minIndex == -1)
                {
                    minIndex = i;
                }
                else if (DistanceToTarget(edges[minIndex].targetNode.Value, targetNode) > distanceToTarget)
                {
                    minIndex = i;
                }
            }

        }
        nextEdge = edges[minIndex];
        return nextEdge;
    }

}
