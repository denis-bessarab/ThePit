using System.Collections;
using UnityEngine;
public class Rope_UsableItem : UsableItem
{
    [Header("Parameters")]
    [SerializeField] private float minRopePower = 0f;
    [SerializeField] private float currentRopePower = 0f;
    [SerializeField] private float maxRopePower = 3f;
    [SerializeField] private float breakPointPercent = 0.3f;
    [SerializeField] private float ropeForce = 10f;
    public Coroutine ropeLoadCoroutine;

    protected override void Start()
    {
        base.Start();
        usableItemUI.SetupUsableItemUI(minRopePower, maxRopePower, true, 0.3f);
    }
    public override void LMBHoldAction()
    {
        ropeLoadCoroutine ??= StartCoroutine(RopeLoadCoroutine());
    }

    private IEnumerator RopeLoadCoroutine()
    {
        usableItemUI.ShowUI();
        currentRopePower = minRopePower;

        while (currentRopePower < maxRopePower)
        {
            currentRopePower += Time.deltaTime;
            usableItemUI.UpdateValue(currentRopePower);
            yield return null;
        }
    }

    public override void LMBReleaseAction()
    {
        usableItemUI.UpdateValue(minRopePower);
        usableItemUI.HideUI();

        if (character == null) return;

        if (ropeLoadCoroutine != null)
        {
            StopCoroutine(ropeLoadCoroutine);
            ropeLoadCoroutine = null;
            if (currentRopePower < maxRopePower * breakPointPercent) return;
        }

        var ropeBall = Instantiate(Resources.Load("Prefabs/Rope/RopeBall") as GameObject);
        ropeBall.transform.position = character.transform.position;

        var mousePos = C_Utility.GetMouseWorldPosition();
        var dir = C_Utility.GetDirectionToPointer(ropeBall.transform.position, mousePos);

        var rb = ropeBall.GetComponent<Rigidbody2D>();
        var force = ropeForce * currentRopePower * dir;
        rb.AddForceAtPosition(force, ropeBall.transform.position, ForceMode2D.Impulse);

        RemoveItemFromInventory(1);
    }

    protected override void RemoveItemFromInventory(int quantity)
    {
        inventory.RemoveItemFromInventory(itemReference, quantity);
    }
}
