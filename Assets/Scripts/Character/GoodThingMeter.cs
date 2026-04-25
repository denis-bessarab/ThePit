using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoodThingMeter : MonoBehaviour
{
    [SerializeField] private List<GameObject> goodThings;
    [SerializeField] private List<GameObject> hints;
    [SerializeField] private GameObject arrowPrefab;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        for (int i = 0; i < hints.Count; i++)
        {
            var dir = UpdateGoodThingDirection(goodThings[i]);
            var pos = FindArrowPosition(dir);
            var rot = FindArrowAngle(pos, goodThings[i].transform.position);
            UpdateHintPositionAndRotation(hints[i], pos, rot);
        }
    }

    private void FindAllGoodThings()
    {
        var container = GameObject.Find("GoodThings");
        if(container == null) return;

        for (int i = 0; i < container.transform.childCount; i++)
        {
            goodThings.Add(container.transform.GetChild(i).gameObject);
        }
    }

    private void Setup()
    {
        FindAllGoodThings();
        DownloadArrowPrefab();

        for (int i = 0; i < goodThings.Count; i++)
        {
            var dir = UpdateGoodThingDirection(goodThings[i]);
            var pos = FindArrowPosition(dir);
            var rot = FindArrowAngle(pos, goodThings[i].transform.position);
            GenerateGoodThingHint(pos, rot);
        }
    }

    private void DownloadArrowPrefab()
    {
        arrowPrefab = Resources.Load("Prefabs/Arrow") as GameObject;
    }

    private Vector3 FindArrowPosition(Vector3 direction)
    {
        return transform.position + direction * 5;
    }

    private Quaternion FindArrowAngle(Vector3 pos, Vector3 goodThingPos)
    {
        Vector3 dir = goodThingPos - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, angle - 90);
        return rotation;
    }

    private void GenerateGoodThingHint(Vector3 pos, Quaternion rotation)
    {
        var hint = Instantiate(arrowPrefab);
        hint.transform.SetPositionAndRotation(pos, rotation);
        hints.Add(hint);
    }

    private void DestroyGoodThingHints()
    {
        for(int i = 0; i < hints.Count; i++)
        {
            Destroy(hints[i]);
        }
    }

    private Vector3 UpdateGoodThingDirection(GameObject go)
    {
        return (go.transform.position - transform.position).normalized;
    }

    private void UpdateHintPositionAndRotation(GameObject hint, Vector3 pos, Quaternion rot)
    {
        hint.gameObject.transform.SetPositionAndRotation(pos, rot);
    }

    public void RemoveGoodThing(GameObject go)
    {
        var index = goodThings.FindIndex(g => g == go);
        Destroy(hints[index]);
        hints.RemoveAt(index);
        goodThings.RemoveAt(index);
        if (goodThings.Count == 0) Debug.Log("You win!");
    }

}
