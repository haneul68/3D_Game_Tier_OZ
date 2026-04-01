using UnityEngine;

public class Pickup_Item : Item_Base
{
    [SerializeField]
    private Orb_Type type;
    [SerializeField]
    private float value = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Check_Item_Type(player);
            Base_Manager.pool_Mng.pool_Dictionary[item_Data.item_ID].Return(this.gameObject);
        }
    }
    private void Check_Item_Type(Player player) 
    {
        switch (type) 
        {
            case Orb_Type.Health:
                player.Heal(value);
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