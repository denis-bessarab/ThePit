using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Location Description", menuName = "PCG/Location Description")]
public class LocationDescription : ScriptableObject
{
    [SerializeField] public GameObject locationPrefab;
    [SerializeField] public List<LocationConnection> connections;
}
