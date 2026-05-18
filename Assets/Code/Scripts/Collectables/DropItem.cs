using System;
using UnityEngine;

[Serializable]
public class DropItem
{
    public GameObject prefab;
    [Range(0,100)]
    public float dropChance;
}
