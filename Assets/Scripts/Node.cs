using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 position;
    private NodeManager nodeManager;
   
    void Start()
    {
        position = transform.position;
        nodeManager = FindNodeManager();
        if(nodeManager != null) nodeManager.RegisterNode(this);
    }

    private NodeManager FindNodeManager()
    {
        return FindFirstObjectByType<NodeManager>();
    }
}
