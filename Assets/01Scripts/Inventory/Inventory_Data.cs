using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Slot
{
    public Item_Scriptable item;  
    public int quantity;           
    public bool IsEmpty => item == null;

    public Inventory_Slot(Item_Scriptable item = null, int quantity = 0)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
public class Inventory_Data
{
    private Dictionary<Item_Type, List<Inventory_Slot>> inventory_Slots = new Dictionary<Item_Type, List<Inventory_Slot>>();

    public IReadOnlyDictionary<Item_Type, List<Inventory_Slot>> Inventory_Slots => inventory_Slots;

    public readonly int default_Slot_Size = 11;
    public void Init()
    {
        foreach (Item_Type type in Enum.GetValues(typeof(Item_Type)))
        {
            if (type == Item_Type.None) continue;
            inventory_Slots[type] = new List<Inventory_Slot>();
            for (int i = 0; i < default_Slot_Size; i++)
                inventory_Slots[type].Add(new Inventory_Slot());
        }
    }

    public void Get_Inventory_Size(Item_Type type, out int size)
    {
        size = inventory_Slots[type].Count;
    }

    public bool Can_Add_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.max_Stack)
            {
                int canAdd = item.max_Stack - slot.quantity;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int canAdd = item.max_Stack;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        return false; 
    }

    public bool Add_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        // 기존 슬롯에 채우기
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.max_Stack)
            {
                int canAdd = Math.Min(item.max_Stack - slot.quantity, remaining);
                slot.quantity += canAdd;
                remaining -= canAdd;
                if (remaining <= 0) return true;
            }
        }

        // 빈 슬롯에 새로 추가
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int toAdd = Math.Min(item.max_Stack, remaining);
                slot.item = item;
                slot.quantity = toAdd;
                remaining -= toAdd;
                if (remaining <= 0) return true;
            }
        }

        // 모든 슬롯이 다 차면 실패
        return false;
    }

    // 슬롯 단위 사용
    public bool Use_Item(Item_Scriptable item, int amount)
    {
        if (!inventory_Slots.ContainsKey(item.item_Type)) return false;

        var slots = inventory_Slots[item.item_Type];
        int remaining = amount;

        foreach (var slot in slots)
        {
            if (slot.item != item) continue;

            if (slot.quantity >= remaining)
            {
                slot.quantity -= remaining;
                if (slot.quantity == 0) slot.item = null;
                return true;
            }
            else
            {
                remaining -= slot.quantity;
                slot.quantity = 0;
                slot.item = null;
            }
        }

        return false; // 사용할 슬롯 부족
    }

    public bool Use_Slot(Item_Type type, int slot_Index, int amount)
    {
        if (!inventory_Slots.ContainsKey(type)) return false;

        var slot = inventory_Slots[type][slot_Index];

        if (slot.IsEmpty || slot.quantity < amount) return false;

        slot.quantity -= amount;
        if (slot.quantity == 0) slot.item = null;

        return true;
    }

    public int Get_Quantity(Item_Scriptable item)
    {
        int total = 0;
        foreach (var slot in inventory_Slots[item.item_Type])
        {
            if (slot.item == item)
                total += slot.quantity;
        }
        return total;
    }
    public int Get_Quantity(string itemID)
    {
        int total = 0;

        foreach (var slotList in inventory_Slots.Values) 
        {
            foreach (var slot in slotList)
            {
                if (slot.item != null && slot.item.item_ID == itemID)
                {
                    total += slot.quantity;
                }
            }
        }
        return total;
    }
}
