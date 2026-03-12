using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbourNodes = new();
    public List<Edge> edges = new();

    private void Awake()
    {
        
    }
    public void FindNeighbourNodes()
    {
        Vector2 pos = transform.position;
        List<Vector2> possiblePositions = new()
        {
            new Vector2(pos.x + 1, pos.y), //RIGHT
            new Vector2(pos.x + 1, pos.y - 1), //ROGHT DOWN
            new Vector2(pos.x, pos.y - 1), // DOWN
            new Vector2(pos.x - 1, pos.y - 1), //LEFT DOWN
            new Vector2(pos.x - 1, pos.y), //LEFT
            new Vector2(pos.x - 1, pos.y + 1), //LEFT UP
            new Vector2(pos.x, pos.y + 1), //UP
            new Vector2(pos.x + 1, pos.y + 1), //RIGHT UP
        };

        for(int i = 0; i < possiblePositions.Count; i++)
        {
            if(NodeManager.nodes.TryGetValue(possiblePositions[i], out Node neighbourNode))
            {
                neighbourNodes.Add(neighbourNode);
            }
        }
    }
}
