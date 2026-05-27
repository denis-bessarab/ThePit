using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccessBar : C_UsableItemsController
{
    [Header("Parameters")]
    [SerializeField] private GameObject usableItemPrefabReference;

    [Header("Components")]
    [SerializeField] private GameObject QABCanvas;
    [SerializeField] private GameObject QABCellsContainer;
    [SerializeField] private List<QuickAccessCell> quickAccessCells;
    [SerializeField] private QuickAccessCell activeCell;
    [SerializeField] public ItemDynamicsController itemDynamicsController;
    public QuickAccessCell ActiveCell
    {
        get => activeCell;
        set
        {
            if(activeCell != null) activeCell.DeactivateCell();
            activeCell = value;
            activeCell.ActivateCell();
            DestroyUsableItemPrefabRef(usableItemPrefabReference);
            if (activeCell.Item == null) return;
            InstantiateUsableItemPrefabRef(activeCell.Item.usableItemPrefab);
        }
    }

    private void Reset()
    {
        SetupQAB();
    }

    private void Start()
    {
        SetActiveCell(0);
        StartCoroutine(FindDynamicItemHolder());
    }

    private void SetupQAB()
    {
        QABCanvas = transform.GetChild(0).gameObject;

        if(QABCanvas == null )
        {
            Debug.LogWarning("Impossible to find QABCanvas GameObject");
            return;
        }

        QABCellsContainer = QABCanvas.transform.GetChild(0).gameObject;

        FindQABCells(QABCellsContainer.transform);
    }

    public void ShowQAB()
    {
        QABCanvas.SetActive(true);
    }

    public void HideQAB()
    {
        QABCanvas.SetActive(false);
    }

    private void FindQABCells(Transform container)
    {
        var cellsAmount = container.childCount;

        for (int i = 0; i < cellsAmount; i++)
        {
            var cell = container.GetChild(i).GetComponent<QuickAccessCell>();
            cell.quickAccessBar = this;
            quickAccessCells.Add(cell);
        }
    }

    public void SetActiveCell(int index)
    {
        var cell = quickAccessCells[index];
        ActiveCell = cell;
    }

    private void InstantiateUsableItemPrefabRef(GameObject prefab)
    {
        if (prefab == null) return;
        usableItemPrefabReference = Instantiate(prefab);
        CurrentUsableItem = usableItemPrefabReference.GetComponent<UsableItem>();
    }

    private void DestroyUsableItemPrefabRef(GameObject prefab)
    {
        if (prefab == null) return;
        Destroy(prefab);
        usableItemPrefabReference = null;
        CurrentUsableItem = null;
    }

    public List<QuickAccessCell> GetQuickAccessBarCells()
    {
        return quickAccessCells;
    }

    private IEnumerator FindDynamicItemHolder()
    {
        while (itemDynamicsController == null)
        {
            itemDynamicsController = FindAnyObjectByType<ItemDynamicsController>();
            yield return null;
        }
    }

    public void SyncActiveCell()
    {
        ActiveCell = ActiveCell;
    }
}
