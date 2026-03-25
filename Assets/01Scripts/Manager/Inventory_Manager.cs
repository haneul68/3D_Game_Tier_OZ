using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager
{
    public int[] Inventory_Size = new int[3];

    public Dictionary<string, Item_Base> usable_Item = new Dictionary<string, Item_Base>();

    public Stack<GameObject> Acrtion_Panal_Holder = new Stack<GameObject>();

    public event Action<Item_Holder> OnItemChanged;

    private void Init() 
    {
        
    }

    public int Get_Item_Count(string item_Name)
    {
        if (Base_Manager.data_Mng.p_Item_Holder.TryGetValue(item_Name, out var item))
        {
            return item.holder.Quantity;
        }
        return 0;
    }

    public bool Consume_Item(string item_Name, int amount) //아이템 소비
    {
        if (Base_Manager.data_Mng.p_Item_Holder.TryGetValue(item_Name, out var item))
        {
            if (item.holder.Quantity >= amount)
            {
                item.holder.Quantity -= amount;

                if (item.holder.Quantity == 0 && usable_Item.ContainsKey(item_Name))
                {
                    usable_Item.Remove(item_Name);
                }

                return true;
            }
        }
        return false; 
    }

    public void Get_Item(Item_Scriptable item, int value) 
    {
        if (Base_Manager.data_Mng.p_Item_Holder.ContainsKey(item.name))
        {
            Base_Manager.data_Mng.p_Item_Holder[item.name].holder.Quantity += value;
            return;
        }
    }
}
