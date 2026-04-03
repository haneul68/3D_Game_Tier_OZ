using System;
using UnityEngine;


public class Item_Base : MonoBehaviour
{
    public Item_Scriptable item_Data;
    public int amount;

    public virtual void Init(Vector3 pos, Item_Scriptable item, int amount = 1) 
    {
        item_Data = item;
        pos.y = 1.0f;
        transform.position = pos;
        this.amount = amount;
    }
}
