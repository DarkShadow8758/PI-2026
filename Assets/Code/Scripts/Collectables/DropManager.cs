using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Drop geral")]
    [Range(0,100)]
    [SerializeField] private float overallDropChance = 50f;

    [Header("Itens possíveis")]
    [SerializeField] private List<DropItem> dropItems;

    public void TryDrop(Vector3 position, NetworkRunner runner)
    {
        if (runner == null) return;
        if (Random.Range(0f,100f) > overallDropChance)
            return;

        NetworkPrefabRef item = GetRandomItem();

        if(item.IsValid)
        {
            runner.Spawn(
                item,
                position,
                Quaternion.identity
            );
        }
    }

    private NetworkPrefabRef GetRandomItem()
    {
        float totalWeight = 0;

        foreach(var item in dropItems)
        {
            totalWeight += item.dropChance;
        }

        float randomValue = Random.Range(0f,totalWeight);

        float current = 0;

        foreach(var item in dropItems)
        {
            current += item.dropChance;

            if(randomValue <= current)
            {
                return item.prefab;
            }
        }

        return new NetworkPrefabRef();
    }
}