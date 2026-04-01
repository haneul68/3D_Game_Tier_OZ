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
    private Dictionary<string, Item_Scriptable> d_Item_Data = new Dictionary<string, Item_Scriptable>();
    public IReadOnlyDictionary<string, Item_Scriptable> Item_Data => d_Item_Data;
    
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

            if (!d_Item_Data.ContainsKey(data.item_ID))
            {
                d_Item_Data.Add(data.item_ID, item);
            }
        }
    }

    public bool Get_Item_Data(string item_Id, out Item_Scriptable item_Data)
    {
        return Item_Data.TryGetValue(item_Id, out item_Data);
    }
}
