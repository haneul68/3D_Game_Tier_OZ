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
        if (!Base_Manager.inventory_Mng.inventory_Logic.Use_Item(item_Data.item_ID, value))
        {
            Debug.Log("사용 불가능");
            return;
        }

        int temp = Base_Manager.inventory_Mng.inventory_Logic.Get_Item_Count(item_Data.item_ID);
        Debug.Log($"남은 개수 : {temp}");
    }
}
