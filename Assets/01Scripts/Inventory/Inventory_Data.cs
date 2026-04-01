using System.Collections.Generic;
using UnityEngine;

public class Inventory_Data
{
    private Dictionary<string, Item_Holder> player_Items = new Dictionary<string, Item_Holder>();

    private Dictionary<string, Item_Base> usable_Item = new Dictionary<string, Item_Base>();

    public IReadOnlyDictionary<string, Item_Holder> Player_Items => player_Items;
    public IReadOnlyDictionary<string, Item_Base> Usable_Item => usable_Item;

    public void Init()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");
        foreach (var data in datas)
        {
            if (!player_Items.ContainsKey(data.item_ID))
            {
                player_Items.Add(data.item_ID, new Item_Holder
                {
                    item_Data = data,
                    holder = new Holder { Quantity = 0 }
                });
            }
        }
    }

    public int Get_Quantity(string itemID)
    {
        return player_Items.TryGetValue(itemID, out Item_Holder holder) ? holder.holder.Quantity : 0;
    }

    public bool Add_Quantity(string item_ID, int amount)
    {
        if (player_Items.TryGetValue(item_ID, out Item_Holder holder))
        {
            holder.holder.Quantity += amount;
            return true;

        }
        return false;
    }

    public bool Set_Quantity(string itemID, int amount)
    {
        if (player_Items.TryGetValue(itemID, out Item_Holder holder))
        {
            holder.holder.Quantity = amount;
            return true;
        }
        return false;
    }
    public bool Add_Usable_Item(string itemID, Item_Base item)
    {
        if (!usable_Item.ContainsKey(itemID))
        {
            usable_Item.Add(itemID, item);
            return true;
        }
        return false;
    }

    public bool Remove_Usable_Item(string itemID)
    {
        return usable_Item.Remove(itemID);
    }
    public bool Has_Usable_Item(string itemID)
    {
        return usable_Item.ContainsKey(itemID);
    }
}
