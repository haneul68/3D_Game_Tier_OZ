using System;
using UnityEngine;

public class Inventory_Logic
{
    private Inventory_Data inventory_Data;

    public event Action<Item_Holder> OnItemChanged;

    public Inventory_Logic(Inventory_Data data)
    {
        inventory_Data = data;
    }

    //public void Try_Get_Item(Item_Scriptable item, int amount)
    //{
    //    string itemID = item.item_ID;
    //    var currentQuantity = inventory_Data.Get_Quantity(itemID);
    //    int maxStack = Mathf.Max(1, item.max_Stack);

    //    int slotIndex = (int)item.item_Type;
    //    int currentSlots = Base_Manager.inventory_Mng.Inventory_Size[slotIndex];

    //    int maxCapacity = currentSlots * maxStack;
    //    int addableAmount = Mathf.Min(amount, maxCapacity - currentQuantity);

    //    if (addableAmount <= 0)
    //    {
    //        return;
    //    }

    //    inventory_Data.Add_Quantity(item.item_ID, amount);
    //    OnItemChanged?.Invoke(inventory_Data.Player_Items[item.item_ID]);
    //}
    public bool Get_Item(Item_Scriptable item, int amount)
    {
        int slot_Index = (int)item.item_Type;
        Base_Manager.inventory_Mng.Get_Inventory_Size(slot_Index, out int maxSlots);

        int current_Quantity = inventory_Data.Get_Quantity(item.item_ID);
        int max_Stack = item.max_Stack;

        int current_Item_Slots = Mathf.CeilToInt((float)current_Quantity / max_Stack);

        int current_Use_Slot = 0;
        foreach (var kvp in inventory_Data.Player_Items)
        {
            if (kvp.Key == item.item_ID) continue;
            int item_Count = kvp.Value.holder.Quantity;
            if (item_Count == 0) continue;
            int item_Max_Stack = kvp.Value.item_Data.max_Stack;
            current_Use_Slot += Mathf.CeilToInt((float)item_Count / item_Max_Stack);
        }

        int available_Slots = maxSlots - current_Use_Slot;

        int total_Quantity_After = current_Quantity + amount;
        int slotsNeededForItem = Mathf.CeilToInt((float)total_Quantity_After / max_Stack);

        int additional_Slots_Needed = slotsNeededForItem - current_Item_Slots;

        if (additional_Slots_Needed <= available_Slots)
        {
            inventory_Data.Add_Quantity(item.item_ID, amount);
            OnItemChanged?.Invoke(inventory_Data.Player_Items[item.item_ID]);
            return true;
        }
        else
        {
            Debug.Log($"인벤토리 슬롯 부족: {item.item_Name}");
            return false;
        }
    }
    public bool Use_Item(string item_ID, int amount)
    {
        int current_Quantity = inventory_Data.Get_Quantity(item_ID);

        if (current_Quantity >= amount)
        {
            if (inventory_Data.Add_Quantity(item_ID, -amount))
            {
                Debug.Log("개수 - 성공");
                OnItemChanged?.Invoke(inventory_Data.Player_Items[item_ID]);
                if (inventory_Data.Get_Quantity(item_ID) == 0 && inventory_Data.Has_Usable_Item(item_ID))
                {
                    inventory_Data.Remove_Usable_Item(item_ID);
                }

                return true;
            }
        }
        return false;
    }

    public int Get_Item_Count(string item_ID)
    {
        return inventory_Data.Get_Quantity(item_ID);
    }
}
