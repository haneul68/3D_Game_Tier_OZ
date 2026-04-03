using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable/HealthPotion")]
public class Health_Potion : Item_Scriptable
{
    public override void Use(IDamageable target)
    {
        target.Heal(item_Value);

        Base_Manager.pool_Mng.Pooling_OBJ("Heal_Effect").Get(effect =>
        {
            Return_S re = effect.GetComponent<Return_S>();
            re.name = "Heal_Effect";
            re.Start_Return_Coroutine();
            if (target is Character character)
            {
                effect.transform.SetParent(character.transform);
                effect.transform.position = character.transform.position;
            }
        });
    }
}
