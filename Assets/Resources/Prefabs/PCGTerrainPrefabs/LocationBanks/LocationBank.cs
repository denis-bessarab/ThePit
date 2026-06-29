using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Location Bank", menuName = "PCG/Location Bank")]
public class LocationBank : ScriptableObject
{
    [SerializeField] public List<LocationDescription> locations;
}
