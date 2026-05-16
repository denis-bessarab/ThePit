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
    public QuickAccessCell ActiveCell
    {
        get => activeCell;
        set
        {
            if(activeCell != null) activeCell.DeactivateCell();
            activeCell = value;
            activeCell.ActivateCell();
        }
    }

    private void Reset()
    {
        SetupQAB();
    }

    private void Start()
    {
        SetActiveCell(0);
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

        if (cell.CellReference == null) return;

        CurrentUsableItem = null;
        Destroy(usableItemPrefabReference);

        var item = cell.CellReference.Item as CustomItem;
        if (item == null) return;

        usableItemPrefabReference = Instantiate(item.usableItemPrefab);
        CurrentUsableItem = usableItemPrefabReference.GetComponent<UsableItem>();
    }
}
