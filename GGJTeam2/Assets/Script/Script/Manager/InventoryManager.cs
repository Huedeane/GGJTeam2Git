using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ItemIntDictionary : SerializableDictionary<Item, int> { }

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private ItemIntDictionary m_InventoryList;

    public ItemIntDictionary InventoryList { get => m_InventoryList; set => m_InventoryList = value; }


    private void OnEnable()
    {
        EventManager.StartListening("ItemAdded", AddItem);
        EventManager.StartListening("ItemRemoved", RemoveItem);
    }

    void AddItem() {
        int outInt;
        if (m_InventoryList.TryGetValue(EventManager.EventItem, out outInt))
        {
            outInt++;
        }
        else {
            m_InventoryList.Add(EventManager.EventItem, 1);
        }
    }
    void RemoveItem()
    {
        int outInt;
        if (m_InventoryList.TryGetValue(EventManager.EventItem, out outInt))
        {
            outInt--;
            if (outInt == 0) {
                InventoryList.Remove(EventManager.EventItem);
            }
        }
        else
        {
            Debug.Log("Item does not exist");
        }
    }

    bool CheckItem(Item item)
    {
        int outInt;
        if (m_InventoryList.TryGetValue(EventManager.EventItem, out outInt))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
