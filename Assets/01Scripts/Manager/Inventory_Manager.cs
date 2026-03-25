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

    public void Get_Item(Item_Scriptable item, int value) 
    {
        if (Base_Manager.data_Mng.p_Item_Holder.ContainsKey(item.name))
        {
            Base_Manager.data_Mng.p_Item_Holder[item.name].holder.Quantity += value;
            return;
        }
    }
}
