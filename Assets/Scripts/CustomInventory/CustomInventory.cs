using System;
using System.Collections;
using UnityEngine;

public class CustomInventory : PGS_Inventory
{
    [SerializeField] private DynamicItemHolder dynamicItemHolder;

    private void Start()
    {
        StartCoroutine(FindDynamicItemHolder());
    }

    protected override void SendAddItemRaport(AddResult result)
    {
        base.SendAddItemRaport(result);
    }

    private IEnumerator FindDynamicItemHolder()
    {
        while (dynamicItemHolder == null)
        {
            dynamicItemHolder = FindAnyObjectByType<DynamicItemHolder>();
            yield return null;
        }
    }
}
