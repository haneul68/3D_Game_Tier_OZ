using UnityEngine;

public class Goblin : Enemy_Base
{
    protected override void Die()
    {
        Base_Manager.pool_Mng.Pooling_OBJ("Health_Orb").Get(obj =>
        {
            Vector3 pos = transform.position;
            pos.y = 1.0f;
            obj.transform.position = pos;
        });

        
        base.Die();
    }
}
