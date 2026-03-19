using UnityEngine;
using UnityEngine.UI;

public class UI_HP_Bar : MonoBehaviour
{
    [SerializeField] private Image hp_Fill; // Fill е╦ют Image

    private IDamageable target;

    public void Set_Target(IDamageable character)
    {
        if (target != null)
            target.On_HP_Change_Event -= Update_HP_Bar;

        target = character;
        target.On_HP_Change_Event += Update_HP_Bar;

        if (target is Character m_Character)
        {
            float currentHP = m_Character.character_Stats.Get_Current_HP;
            float maxHP = m_Character.character_Stats.Get_Max_HP;
            Update_HP_Bar(currentHP, maxHP);
        }
    }

    private void Update_HP_Bar(float currentHP, float maxHP)
    {
        hp_Fill.fillAmount = currentHP / maxHP;
    }

    private void OnDestroy()
    {
        if (target != null)
            target.On_HP_Change_Event -= Update_HP_Bar;
    }
}
