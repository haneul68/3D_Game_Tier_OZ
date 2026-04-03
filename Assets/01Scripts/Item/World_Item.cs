using UnityEngine;

public class World_Item : Item_Base
{
    [SerializeField]
    private World_Item_Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player)) return;

        if (!Base_Manager.inventory_Mng.inventory_Data.Can_Add_Item(item_Data, amount))
        {
            Debug.Log($"공간 부족: {item_Data.item_Name}");
            return;
        }

        bool success = Base_Manager.inventory_Mng.inventory_Logic.Get_Item(item_Data, amount);
        if (!success)
        {
            Debug.Log($"인벤토리 추가 실패: {item_Data.item_Name}");
            return;
        }

        Base_Manager.pool_Mng.pool_Dictionary[item_Data.item_ID].Return(this.gameObject);
    }
}
