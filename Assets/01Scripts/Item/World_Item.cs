using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class World_Item : Item_Base
{
    [SerializeField]
    private World_Item_Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Base_Manager.pool_Mng.pool_Dictionary[item_Data.item_Name].Return(this.gameObject);
            Base_Manager.inventory_Mng.Get_Item(item_Data,1);
            if (!Base_Manager.inventory_Mng.usable_Item.ContainsKey(item_Data.item_Name))
            {
                if (item_Data.item_Type == Item_Type.Consumable)
                {
                    Base_Manager.inventory_Mng.usable_Item.Add(item_Data.item_Name, this);
                }
            }
        }
    }
    public override void Use_Item(int value)
    {
        Check_Item_Type(Base_Manager.instance.current_Player);
        base.Use_Item(value);
        
    }
    private void Check_Item_Type(Player player)
    {
        switch (type)
        {
            case World_Item_Type.Health:
                player.Heal(item_Data.item_Value);
                Base_Manager.pool_Mng.Pooling_OBJ("Heal_Effect").Get(effect =>
                {
                    Return_S re = effect.GetComponent<Return_S>();
                    re.name = "Heal_Effect";
                    re.Start_Return_Coroutine();
                    effect.transform.SetParent(player.transform);
                    effect.transform.position = player.transform.position;
                });
                break;
        }
    }
}
