using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LocationConnection
{
    [SerializeField] public Vector3 connectionPosition;
    [SerializeField] public List<ConnectionType> possibleConnectionTypes;
}
