using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AStar))]
public class EnemyMovementSystem : MonoBehaviour
{
    private Dictionary<Vector2, Node> nodes = new();
    private bool nodesCopyCreated = false;
    private AStar aStar;
    [SerializeField] private float movingSpeed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform target;
    [SerializeField] private List<Node> path;

    private Coroutine movingCoroutine;

    private void Awake()
    {
        aStar = GetComponent<AStar>();
    }

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
        path = CalculatePath();
        StartCoroutine(FollowThePath(path));

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CalculatePath();
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
        foreach(var node in nodes)
        {
            var neighbourNodes = node.Value.neighbourNodes;

            for (int i = 0; i < neighbourNodes.Count; i++)
            {
                var targetNode = new KeyValuePair<Vector2, Node>(neighbourNodes[i].position, neighbourNodes[i]);
                EdgeType edgeType = DefineEdgeType();
                var edge = new Edge(targetNode, edgeType);
                node.Value.edges.Add(edge);
            }
        }
    }

    private EdgeType DefineEdgeType()
    {
        return EdgeType.Run;
    }

    private List<Node> CalculatePath()
    {
        var startNode = FindClosestNode(transform.position);
        var targetNode = FindClosestNode(target.position);
        path = aStar.FindPath(startNode, targetNode);
        return path;
    }

    private Node FindClosestNode(Vector2 pos)
    {
        var floorPos = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        if (nodes.TryGetValue(floorPos, out Node node))
        {
            return node;
        }
        else
        {
            Debug.LogWarning($"Trying to get unregistered node {floorPos} for {pos}");
            return null;
        }
    }

    private IEnumerator FollowThePath(List<Node> path)
    {
        Queue<Node> nodesQueue = new();

        for(int i = 0; i < path.Count; i++)
        {
            nodesQueue.Enqueue(path[i]);
        }

        while (nodesQueue.Count > 0)
        {
            var nextNode = nodesQueue.Dequeue();
            yield return StartCoroutine(Move(nextNode));
        }

        movingCoroutine = null;
        Debug.Log("Traget reached");
    }

    private IEnumerator Move(Node node)
    {
        while(Vector2.Distance(transform.position, node.position) > 0.1f)
        {
            var dir = (node.position - new Vector2(transform.position.x, transform.position.y)).normalized;
            var dirX = Mathf.Round(dir.x);
            _rigidbody2D.linearVelocityX = movingSpeed * dirX;
            yield return null;
        }
        _rigidbody2D.linearVelocityX = 0;
        movingCoroutine = null;
    }
}
