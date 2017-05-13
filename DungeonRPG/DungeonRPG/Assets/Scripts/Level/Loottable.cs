using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

class Loottable
{
    int minItems = 100;
    int maxItems = 120;
    int amount;

    List<GameObject> items;

    public void Initialize(int minItems, int maxItems)
    {
        this.minItems = minItems;
        this.maxItems = maxItems;

        amount = Random.Range(minItems, maxItems + 1);

        items = new List<GameObject>();

        ItemManager.Instance.Factory.GetItemInstances(amount).HandleAction(i => items.Add(i.gameObject));
    }

    public void DropItems(Vector3 pos)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(true);
            items[i].gameObject.transform.position = RandomDropPosition(pos);
            items[i].GetComponent<ItemInstance>().Drop();
        }

        items.HandleAction(i => i.gameObject.SetActive(true));

        items.HandleAction(i => i.gameObject.transform.position = RandomDropPosition(pos));
    }

    private Vector3 RandomDropPosition(Vector3 pos)
    {
        return new Vector3(Random.Range(pos.x - 1f, pos.x + 1f), 0.3f, Random.Range(pos.z - 1f, pos.z + 1f));
    }
}

