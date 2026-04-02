using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AStar))]
public class E_MovementSystem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float movingSpeed = 2f;
    [SerializeField] private float pathRecalcTime = 1.0f;
    [SerializeField] private List<Node> path;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private AStar aStar;
    [SerializeField] private Enemy enemy;

    private Dictionary<Vector2, Node> nodes = new();
    private bool nodesCopyCreated = false;

    private Coroutine followThePathCoroutine;
    private Coroutine movingToNodeCoroutine;
    public Coroutine calculatePathCoroutine;
    public List<Node> Path
    {
        get => path;
        set
        {
            path = value;

            EMSStop();

            if (path == null) return;

            followThePathCoroutine = StartCoroutine(FollowThePath(path));
        }
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EMSStop();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            EMSStart();
        }
    }

    private IEnumerator WaitForNodeManager()
    {
        while (!NodeManager.Ready)
        {
            yield return null;
        }
        nodes = CreateNodesCopy();
    }

    private IEnumerator WaitForNodesCopy()
    {
        while (!nodesCopyCreated)
        {
            yield return null;
        }
        CalculateEdges();
    }

    private Dictionary<Vector2, Node> CreateNodesCopy()
    {
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

    public IEnumerator CalculatePath(Transform target)
    {
        //SETTING VARIABLES FOR NODES
        Node startNode = null;
        Node targetNode = null;

        //GETTING DIRECTION FROM ENEMY TO TARGET
        Vector2 dir = (target.position - transform.position).normalized;

        //STARTING COROUTINES OF FINDING PATH NODES FOR TARGET AND ENEMY
        StartCoroutine(TrackTarget(target, dir, node => targetNode = node));
        StartCoroutine(TrackTarget(transform, dir, node => startNode = node));

        //REPEATING TILL NODES WILL FOUND
        while (startNode == null || targetNode == null)
        {
            yield return null;
        }

        //SETTING PATH
        Path = aStar.FindPath(startNode, targetNode);

        //RECALCULATE PATH IN pathRecalcTime SECONDS
        yield return new WaitForSeconds(pathRecalcTime);

        if (enemy.Target != null)
        {
            RecalculatePath();
        }
    }

    private void RecalculatePath()
    {
        Path = null;

        if (calculatePathCoroutine != null)
        {
            StopCoroutine(calculatePathCoroutine);
            calculatePathCoroutine = null;
        }

        if (enemy.Target != null)
        {
            calculatePathCoroutine = StartCoroutine(CalculatePath(enemy.Target));
        }
    }

    public IEnumerator TrackTarget(Transform target, Vector2 dir, System.Action<Node> setNode)
    {
        //THIS FUNCTION EXISTS FOR TRACKING TARGET IF IT IS IMPOSSIBLE TO GET CLOSEST NODE NOW
        //IT CONTINUES TO TRY GET AVAILABLE NODE
        //DIRECTION IS NEEDED TO GET CLOSEST NODE IN THIS DIRECTION AND PREVENT TURNING BACK AFTER EVERY CALCULATION
        Node nodeClosestToTarget = null;

        while (nodeClosestToTarget == null)
        {
            nodeClosestToTarget = FindClosestNode(target.position, dir);
            yield return null;
        }

        setNode(nodeClosestToTarget);
    }

    private Node FindClosestNode(Vector2 pos, Vector2 dir)
    {
        Vector2 posInt = Vector2.zero;

        if(dir.x < 0)
        {
            posInt = new Vector2(Mathf.Floor(pos.x), Mathf.Floor(pos.y));
        }

        if (dir.x > 0)
        {
            posInt = new Vector2(Mathf.Ceil(pos.x), Mathf.Floor(pos.y));
        }

        if (nodes.TryGetValue(posInt, out Node node))
        {
            return node;
        }
        else
        {
            //Debug.LogWarning($"Trying to get unregistered node {posInt} for {pos}");
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
            movingToNodeCoroutine = StartCoroutine(Move(nextNode));
            yield return movingToNodeCoroutine;
        }

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
        followThePathCoroutine = null;
    }

    public void EMSStop(bool full = false)
    {
        if (calculatePathCoroutine != null && full)
        {
            StopCoroutine(calculatePathCoroutine);
            calculatePathCoroutine = null;
        }

        if (followThePathCoroutine != null)
        {
            StopCoroutine(followThePathCoroutine);
            followThePathCoroutine = null;
        }

        if (movingToNodeCoroutine != null)
        {
            StopCoroutine(movingToNodeCoroutine);
            movingToNodeCoroutine = null;
        }
    }

    public void EMSStart()
    {
        if(enemy.Target != null)
        {
            calculatePathCoroutine = StartCoroutine(CalculatePath(enemy.Target));
        }
    }
}
