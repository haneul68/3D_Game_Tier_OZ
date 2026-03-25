using UnityEngine;

public class Goblin : Enemy_Base
{
    protected override void Die()
    {
        Base_Manager.pool_Mng.Pooling_OBJ("Health_Orb").Get(obj =>
        {
            obj.GetComponent<Item_Base>().Init(transform.position, Base_Manager.data_Mng.d_Item_Data["Health_Orb"]);
        });

        
        base.Die();
    }
}
