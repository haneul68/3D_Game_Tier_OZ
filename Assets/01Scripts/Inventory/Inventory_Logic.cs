using System;
using UnityEngine;

public class Inventory_Logic
{
    private Inventory_Data inventory_Data;

    public event Action<Item_Scriptable> OnItemChanged;

    public Inventory_Logic(Inventory_Data data)
    {
        inventory_Data = data;
    }
    public bool Get_Item(Item_Scriptable item, int amount)
    {
        bool success = inventory_Data.Add_Item(item, amount);
        if (success) OnItemChanged?.Invoke(item);
        return success;
    }

    public bool Use_Item(Item_Scriptable item, int amount)
    {
        bool success = inventory_Data.Use_Item(item, amount);

        if (!success) return false;

        item.Use(Base_Manager.instance.current_Player);

        OnItemChanged?.Invoke(item);
        return true;
    }

    public int Get_Item_Count(string item_ID)
    {
        return inventory_Data.Get_Quantity(item_ID);
    }
}
