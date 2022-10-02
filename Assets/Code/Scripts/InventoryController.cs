using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    [SerializeField] private Item[] itemIndex;

    private List<int> inventory = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < itemIndex.Length; i++) inventory.Add(0);
    }

    public bool hasItem(int itemIndex)
    {
        return inventory[itemIndex] > 0;
    }

    public bool useItem(int itemIndex)
    {
        if (!hasItem(itemIndex)) return false;
        inventory[itemIndex]--;
        return true;
    }

    public void addItem(int itemIndex, int amount)
    {
        inventory[itemIndex] += amount;
    }

    public int getItemIndex(Item item)
    {
        return System.Array.FindIndex(itemIndex, o => item.name == o.name);
    }

    // -------------------------------

    public bool hasItem(Item item)
    {
        return hasItem(getItemIndex(item));
    }

    public bool useItem(Item item)
    {
        return useItem(getItemIndex(item));
    }

    public void removeItem(int itemIndex)
    {
        useItem(itemIndex);
    }

    public void removeItem(Item item)
    {
        useItem(item);
    }

    public void addItem(Item item)
    {
        addItem(item, 1);
    }

    public void addItem(int itemIndex)
    {
        addItem(itemIndex, 1);
    }

    public void addItem(Item item, int amount)
    {
        addItem(getItemIndex(item), amount);
    }
}
