using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager
{
    public int[] Inventory_Size = new int[3];

    public readonly int inv_Default_Size = 11;

    public Stack<GameObject> Acrtion_Panal_Holder = new Stack<GameObject>();

    public readonly Inventory_Data inventory_Data = new Inventory_Data();
    public Inventory_Logic inventory_Logic;

    public void Init()
    {
        inventory_Data.Init();
        inventory_Logic = new Inventory_Logic(inventory_Data);
    }

    public void Get_Inventory_Size(int index, out int size) 
    {
        size = (Inventory_Size[index] + 1) * inv_Default_Size;
    }
}
