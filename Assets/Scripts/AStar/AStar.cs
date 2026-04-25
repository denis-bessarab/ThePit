using UnityEngine;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{
    private Queue<Node> openList = new();
    [SerializeField] private List<Node> closedList = new();

    private float DistanceToTarget(Node currentNode,Node target)
    {
        return Vector2.Distance(currentNode.position, target.position);
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        closedList.Clear();
        openList.Clear();

        List<Node> path = new();

        openList.Enqueue(startNode);
        path.Add(startNode);

        while (openList.Count > 0)
        {
            var currentNode = openList.Dequeue();
            if (currentNode.position == targetNode.position) return path;

            var edges = currentNode.edges;
            var nextEdge = FindEdgeWithMaxWeight(currentNode, targetNode, edges, closedList);

            if(nextEdge == null)
            {
                Debug.Log("Target is unreachable");
                return path;
            }

            closedList.Add(currentNode);

            openList.Enqueue(nextEdge.targetNode.Value);
            path.Add(nextEdge.targetNode.Value);
        }

        return path;
    }

    private Edge FindEdgeWithMaxWeight(Node currentNode, Node targetNode, List<Edge> edges, List<Node> closedList)
    {
        //THIS FUNCTION FINDS MOST EFFICIENT EDGE
        Edge nextEdge;

        //CURRENT DISTANCE TO TARGET NODE
        var currentDistance = DistanceToTarget(currentNode, targetNode);
        int nextEdgeIndex = -1;

        for (int i = 0; i < edges.Count; i++)
        {
            var edge = edges[i];

            //IF EDGE IS ALREADY VIEWED CONTINUE
            if (closedList.Contains(edge.targetNode.Value)) continue;

            //SET INITIAL EDGE INDEX
            if (i == 0) { nextEdgeIndex = i; continue; }

            //DISTANCE FROM EDGE TARGET NODE TO TARGET
            var distanceToTarget = DistanceToTarget(edge.targetNode.Value, targetNode);

            //SET NEW EDGE INDEX IF DISTANCE FROM THIS TARGET NODE IS LESS
            if (distanceToTarget < currentDistance)
            {
                nextEdgeIndex = i;
            }
        }

        if (nextEdgeIndex == -1) return null;

        nextEdge = edges[nextEdgeIndex];
        return nextEdge;
    }
}
