using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccessBar : C_UsableItemsController
{
    [Header("Resources")]
    [SerializeField] private GameObject usableItemPrefabReference;

    [Header("Components")]
    [SerializeField] private GameObject QABCanvas;
    [SerializeField] private GameObject QABCellsContainer;
    [SerializeField] private List<QuickAccessCell> quickAccessCells;
    [SerializeField] private QuickAccessCell activeCell;
    [SerializeField] public ItemDynamicsController itemDynamicsController;
    [SerializeField] public InputManager inputManager;

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

    private void OnEnable()
    {
        SetActiveCell(0);
        StartCoroutine(FindDynamicItemHolder());
        StartCoroutine(FindInputManager(SubscribeToInputManager));
    }
    private void OnDisable()
    {
        UnsubscribeFromInputManager();
    }
    private void OnDestroy()
    {
        UnsubscribeFromInputManager();
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

    private IEnumerator FindInputManager(Action callback)
    {
        while (inputManager == null)
        {
            inputManager = InputManager.Instance;
            yield return null;
        }

        callback?.Invoke();
    }

    public void SyncActiveCell()
    {
        ActiveCell = ActiveCell;
    }

    private void SubscribeToInputManager()
    {
        var im = inputManager;

        im.onQab1.Insert(0, SetActiveCell);
        im.onQab2.Insert(0, SetActiveCell);
        im.onQab3.Insert(0, SetActiveCell);
        im.onQab4.Insert(0, SetActiveCell);
        im.onQab5.Insert(0, SetActiveCell);
        im.onQab6.Insert(0, SetActiveCell);
    }

    private void UnsubscribeFromInputManager()
    {
        var im = inputManager;

        im.onQab1.Remove(SetActiveCell);
        im.onQab2.Remove(SetActiveCell);
        im.onQab3.Remove(SetActiveCell);
        im.onQab4.Remove(SetActiveCell);
        im.onQab5.Remove(SetActiveCell);
        im.onQab6.Remove(SetActiveCell);
    }
}
