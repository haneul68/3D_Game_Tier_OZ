using System;
using UnityEngine;


public class Item_Base : MonoBehaviour
{
    public Item_Scriptable item_Data;

    public virtual void Init(Vector3 pos, Item_Scriptable item) 
    {
        item_Data = item;
        pos.y = 1.0f;
        transform.position = pos;
    }

    public virtual void Use_Item(int value) 
    {
        if (Base_Manager.data_Mng.p_Item_Holder[item_Data.item_Name].holder.Quantity < value) 
        {
            Debug.Log("사용 불가능");
            return;
        }

        Base_Manager.data_Mng.p_Item_Holder[item_Data.item_Name].holder.Quantity -= value;
        Debug.Log($"남은 개수 {Base_Manager.data_Mng.p_Item_Holder[item_Data.item_Name].holder.Quantity}");
        if (Base_Manager.data_Mng.p_Item_Holder[item_Data.item_Name].holder.Quantity == 0) 
        {
            Base_Manager.inventory_Mng.usable_Item.Remove(item_Data.item_Name);
        }
    }
}
