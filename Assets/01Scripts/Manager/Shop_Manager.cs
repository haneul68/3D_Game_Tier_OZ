using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Manager : MonoBehaviour
{
    public Stack<GameObject> Shop_Acrtion_Panal_Holder = new Stack<GameObject>();

    public event Action<Item_Scriptable> OnShopItemChanged;

    public Shop_Interact_OBJ current_Shop;

    public bool Try_Buy_Item(string item_ID, int amount)
    {
        if (!current_Shop.Shop_Item_Datas.ContainsKey(item_ID))
        {
            Debug.Log($"아이템 없음");
            return false;
        }

        var item = current_Shop.Shop_Item_Datas[item_ID];

        if (!Base_Manager.inventory_Mng.inventory_Data.Can_Add_Item(item.data, amount))
        {
            Base_Canvas.instance.Get_Text_Pop_Up("인벤토리 공간이 부족합니다", Color.red);
            return false;
        }

        if (item.total_Amount < amount)
        {
            Base_Canvas.instance.Get_Text_Pop_Up("재고가 없습니다", Color.red);
            return false;
        }

        if (!Base_Manager.data_Mng.Spend_Gold(item.price))
        {
            Base_Canvas.instance.Get_Text_Pop_Up("재화가 부족합니다", Color.red);
            return false;
        }

        if (!Base_Manager.inventory_Mng.inventory_Logic.Get_Item(item.data, amount))
        {
            Debug.Log($"구매 실패");
            return false;
        }

        item.total_Amount -= amount;
        OnShopItemChanged?.Invoke(item.data);
        return true;
    }
}
