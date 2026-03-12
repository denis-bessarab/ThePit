using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSystem : MonoBehaviour
{
    private Dictionary<Vector2, Node> nodes = new();
    private bool nodesCopyCreated = false;

    private void Start()
    {
        if (NodeManager.Ready)
        {
            nodes = CreateNodesCopy();
        }
        else
        {
            StartCoroutine(WaitForNodeManager());
        }

        if (nodesCopyCreated)
        {
            CalculateEdges();
        }
        else
        {
            StartCoroutine(WaitForNodesCopy());
        }
    }

    private IEnumerator WaitForNodeManager()
    {
        Debug.Log("Waiting for Node Manager");
        while (!NodeManager.Ready)
        {
            yield return null;
        }
        CreateNodesCopy();
    }

    private IEnumerator WaitForNodesCopy()
    {
        Debug.Log("Waiting for nodes copy");
        while (!nodesCopyCreated)
        {
            yield return null;
        }
        CalculateEdges();
    }

    private Dictionary<Vector2, Node> CreateNodesCopy()
    {
        Debug.Log("Nodes copy created");
        nodesCopyCreated = true;
        return new Dictionary<Vector2, Node>(NodeManager.nodes);
    }

    private void CalculateEdges()
    {
        Debug.Log($"Edges are calculated");
    }
}
