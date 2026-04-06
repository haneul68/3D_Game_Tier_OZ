using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Action_Slot_Panel : MonoBehaviour, IPointerExitHandler
{
    [SerializeField]
    private Button buy_Button, Exit_Button;

    private Shop_Item_Data current_Item;

    private Shop_Item_Slot current_Slot;

    private bool is_Processing = false;

    public void Init(Shop_Item_Data item, Shop_Item_Slot slot)
    {
        current_Item = item;
        current_Slot = slot;
        is_Processing = false;
        Exit_Button.onClick.RemoveAllListeners();
        Exit_Button.onClick.AddListener(() =>
         {
             Close_Panel();
         });

        buy_Button.onClick.RemoveAllListeners();
        buy_Button.onClick.AddListener(() =>
        {
            Buy_Item();
        });
    }

    private void Buy_Item()
    {
        if (is_Processing) return;

        is_Processing = true;

        Shop_Interact_OBJ shop_Obj = Base_Manager.shop_Mng.current_Shop;
        if (shop_Obj == null) 
        {
            is_Processing = false;
            return;
        }

        if (Base_Manager.shop_Mng.Try_Buy_Item(current_Item.item_ID, 1))
        {
            //current_Item.total_Amount = shop_Obj.Shop_Item_Datas[current_Item.item_ID].total_Amount;

            //current_Slot.Init(current_Item, null, current_Slot.Slot_Index);

            Debug.Log("구매 성공, 남은 수량: " + current_Item.total_Amount);
            Close_Panel();
            return;
        }
        Close_Panel();
        Debug.Log("구매 실패");
    }

    public void Close_Panel()
    {
        Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.SHOP_ITEM_ACTION_PANEL].Return(this.gameObject);
        Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Clear();
        current_Item = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Close_Panel();
    }
}
