using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        Base_Manager.inventory_Mng.usable_Item[current_Item.item_Name].Use_Item(1); // 임시 사용 개수 지정 : 1개 

        if (!Base_Manager.inventory_Mng.usable_Item.ContainsKey(current_Item.item_Name))
        {
            current_Slot.Init(null);
        }
        else
        {
            Item_Holder item = Base_Manager.data_Mng.p_Item_Holder[current_Item.item_Name];

            current_Slot.Init(item);
        }

        Close_Panel();
    }

    private void Close_Panel()
    {
        Base_Manager.pool_Mng.pool_Dictionary["Item_Action_Panel"].Return(this.gameObject);
        Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Clear();
        current_Item = null;
    }
}
