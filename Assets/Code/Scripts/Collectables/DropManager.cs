using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Drop geral")]
    [Range(0,100)]
    [SerializeField] private float overallDropChance = 50f;

    [Header("Itens possíveis")]
    [SerializeField] private List<DropItem> dropItems;

    public void TryDrop(Vector3 position)
    {
        if (Random.Range(0f,100f) > overallDropChance)
            return;

        GameObject item = GetRandomItem();

        if(item != null)
        {
            Instantiate(
                item,
                position,
                Quaternion.identity
            );
        }
    }

    private GameObject GetRandomItem()
    {
        float totalWeight = 0;

        foreach(var item in dropItems)
        {
            totalWeight += item.dropChance;
        }

        float randomValue =
            Random.Range(0f,totalWeight);

        float current = 0;

        foreach(var item in dropItems)
        {
            current += item.dropChance;

            if(randomValue <= current)
            {
                return item.prefab;
            }
        }

        return null;
    }
}