using UnityEngine;

public class MainCamera : MonoBehaviour
{

    [SerializeField] private Transform objectTransform;

    void LateUpdate()
    {
        FollowObject();
    }

    private void FollowObject()
    {
        transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y, -10);
    }
}
