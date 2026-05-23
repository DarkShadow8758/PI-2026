using System;
using UnityEngine;
using Fusion;

[Serializable]
public class DropItem
{
    public NetworkPrefabRef prefab;
    [Range(0,100)]
    public float dropChance;
}
