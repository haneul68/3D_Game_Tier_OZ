using UnityEngine;

[CreateAssetMenu(fileName = "Item_Scriptable", menuName = "Scriptable Objects/Item_Scriptable")]
public class Item_Scriptable : ScriptableObject
{
    public string item_ID;
    public string item_Name;
    public string item_Description;
    public bool is_Pickup = false;
    public float item_Value;
    public int max_Stack;

    public Item_Type item_Type = Item_Type.None;
}
