using System.Collections.Generic;
using UnityEngine;

public class Item_Holder
{
    public Item_Scriptable item_Data;
    public Holder holder;
}

public class Holder 
{ 
    public int Quantity;
}

public class Data_Manager
{
    public Dictionary<string, Item_Scriptable> d_Item_Data = new Dictionary<string, Item_Scriptable>(); 
    public Dictionary<string, Item_Holder> p_Item_Holder = new Dictionary<string, Item_Holder>();

    public void Init() 
    {
        Set_Item_Data();
    }

    private void Set_Item_Data()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        foreach (var data in datas)
        {
            var item = new Item_Scriptable();
            item = data;

            if (!d_Item_Data.ContainsKey(data.name))
            {
                d_Item_Data.Add(data.name, item);
            }

            if (!p_Item_Holder.ContainsKey(data.name))
            {
                Item_Holder itemHolder = new Item_Holder();
                itemHolder.item_Data = data;
                itemHolder.holder = new Holder();
                p_Item_Holder.Add(data.name, itemHolder);
            }
            else
            {
                //TODO: 저장 되어있던 값 p_Item_Holder에 할당
            }
        }
    }
}
