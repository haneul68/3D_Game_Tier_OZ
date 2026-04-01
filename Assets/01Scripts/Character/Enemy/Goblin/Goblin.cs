using UnityEngine;

public class Goblin : Enemy_Base
{
    protected override void Die()
    {
        if (Base_Manager.data_Mng.Get_Item_Data("orb_health", out var item_Data))
        {
            Base_Manager.pool_Mng.Pooling_OBJ("orb_health").Get(obj =>
            {
                 obj.GetComponent<Item_Base>().Init(transform.position, item_Data);
            });
        }
        else
        {
            Debug.LogError("아이템 데이터가 없습니다.");
        }

        base.Die();
    }
}
