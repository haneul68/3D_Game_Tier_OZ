using UnityEngine;


public class Item_Base : MonoBehaviour
{
    public string item_ID;
    public string item_Name;
    public string item_Description;
    public int amount = 1;
    public bool is_Pickup = false;

    public Item_Type item_Type = Item_Type.None;
}
