using UnityEngine;
using UnityEngine.UI;

public class Action_Slot_Panel : MonoBehaviour
{
    [SerializeField]
    private Button use_Button, equip_Button, Exit_Button;

    private Item_Scriptable current_Item;

    private Inventory_Item_Slot current_Slot;

    public void Init(Item_Scriptable item, Inventory_Item_Slot slot)
    {
        current_Item = item;
        current_Slot = slot;
        Exit_Button.onClick.RemoveAllListeners();
        Exit_Button.onClick.AddListener(() =>
         {
             Close_Panel();
         });

        use_Button.onClick.RemoveAllListeners();
        use_Button.onClick.AddListener(() =>
        {
            Use_in_Inventory_Item();
        });
    }

    private void Use_in_Inventory_Item()
    {
        var inventory = Base_Manager.inventory_Mng.inventory_Data;
        var slot = inventory.Inventory_Slots[current_Item.item_Type][current_Slot.Slot_Index];

        bool success = inventory.Use_Slot(current_Item.item_Type, current_Slot.Slot_Index, 1);
        if (!success)
        {
            Debug.Log("아이템 사용 실패");
            return;
        }

        current_Item.Use(Base_Manager.instance.current_Player);

        if (slot.quantity > 0)
            current_Slot.Init(slot.item, slot.quantity, null, current_Slot.Slot_Index);
        else
            current_Slot.Init(null); 

        Close_Panel();
    }
    public void Close_Panel()
    {
        Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.ITEM_ACTION_PANEL].Return(this.gameObject);
        Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Clear();
        current_Item = null;
    }
}
