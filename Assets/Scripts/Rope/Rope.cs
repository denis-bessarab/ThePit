using UnityEngine;
[RequireComponent(typeof (Rope_InputController))]
[RequireComponent(typeof (Rope_Parameters))]
[RequireComponent(typeof (Rope_DataCollector))]
[RequireComponent(typeof (Rope_InputResolver))]
[RequireComponent(typeof (Rope_Actions))]
public class Rope : MonoBehaviour
{
    [SerializeField] private Rope_InputController inputController;
    [SerializeField] private Rope_Parameters parameters;
    [SerializeField] private Rope_DataCollector dataCollector;
    [SerializeField] private Rope_InputResolver inputResolver;
    [SerializeField] private Rope_Actions actions;
    [SerializeField] private DistanceJoint2D distanceJoint2D;
    [SerializeField] private GameObject ropeCenter;
    [SerializeField] private BoxCollider2D ropeCenterCollider;
    [SerializeField] private GameObject ropeEnd;
    [SerializeField] private Rigidbody2D ropeEndRigidbody;
    [SerializeField] private RopeData ropeData;
    [SerializeField] private Character character;

    private void Reset()
    {
        inputController = GetComponent<Rope_InputController>();
        parameters = GetComponent<Rope_Parameters>();
        dataCollector = GetComponent<Rope_DataCollector>();
        inputResolver = GetComponent<Rope_InputResolver>();
        actions = GetComponent<Rope_Actions>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();

        ropeCenter = transform.GetChild(0).gameObject;
        ropeEnd = transform.GetChild(1).gameObject;

        ropeCenterCollider = ropeCenter.GetComponent<BoxCollider2D>();
        ropeEndRigidbody = ropeEnd.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ropeData = dataCollector.UpdateRopeData(transform.gameObject, ropeEnd, distanceJoint2D);
        UpdateRopeCenterState(ropeData);

        if (!inputController.enabled) return;
        inputResolver.ResolveInput(inputController, actions, parameters, distanceJoint2D, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Character")
        {
            var rb = collision.GetComponent<Rigidbody2D>();
            var c = collision.GetComponent<Character>();

            ActivateRope(rb, c);
        }
    }

    private void ActivateRope(Rigidbody2D rb, Character c)
    {
        FreezeRopeEnd();
        distanceJoint2D.connectedBody = rb;
        inputController.enabled = true;
        distanceJoint2D.distance = Vector3.Distance(transform.position, c.gameObject.transform.position);
        character = c;
        character.rope = this;
    }

    public void DeactivateRope()
    {
        UnfreezeRopeEnd();
        distanceJoint2D.connectedBody = ropeEndRigidbody;
        inputController.enabled = false;
        character.rope = null;
        character = null;
        distanceJoint2D.distance = parameters.maxRopeLenght;
    }

    private void UpdateRopeCenterState(RopeData rd)
    {
        ropeCenter.transform.SetPositionAndRotation(rd.ropeCenterPosition, rd.ropeCenterRotation);
        ropeCenterCollider.size = new Vector2(0.2f, rd.ropeLength);
    }

    private void FreezeRopeEnd()
    {
        ropeEnd.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        ropeEnd.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void UnfreezeRopeEnd()
    {
        ropeEnd.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ropeEnd.GetComponent<SpriteRenderer>().enabled = true;
    }
}
