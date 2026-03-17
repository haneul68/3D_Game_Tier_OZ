using UnityEngine;

[System.Serializable]
public class Character_Stats
{
    #region Base_Value
    [SerializeField] 
    private Stat Max_HP = new Stat();
    [SerializeField]
    private Stat Max_Stamina = new Stat();
    [SerializeField] 
    private Stat Max_Mana = new Stat();
    [SerializeField] 
    private Stat Move_Speed = new Stat();
    [SerializeField] 
    private Stat Attack_Power = new Stat();
    #endregion

    #region Base_Current_Value
    [SerializeField]
    private float Current_HP;

    [SerializeField]
    private float Current_Stamina;

    [SerializeField]
    private float Current_Mana;
    #endregion

    #region Get_Value(Stat)
    public float Get_Max_HP => Max_HP.Final_Value;
    public float Get_Max_Stamina => Max_Stamina.Final_Value;
    public float Get_Max_Mana => Max_Mana.Final_Value;
    public float Get_Move_Speed => Move_Speed.Final_Value;
    public float Get_Attack_Power => Attack_Power.Final_Value;
    public float Get_Current_HP => Current_HP;
    public float Get_Current_Stamina => Current_Stamina;
    public float Get_Current_Mana => Current_Mana;
    #endregion

    public void Init()
    {
        Current_HP = Max_HP.Final_Value;
        Current_Stamina = Max_Stamina.Final_Value;
        Current_Mana = Max_Mana.Final_Value;
    }

    public void Clamp_Stats()
    {
        Current_HP = Mathf.Clamp(Current_HP, 0, Max_HP.Final_Value);
        Current_Stamina = Mathf.Clamp(Current_Stamina, 0, Max_Stamina.Final_Value);
        Current_Mana = Mathf.Clamp(Current_Mana, 0, Max_Mana.Final_Value);
    }

    public void Take_HP_Damage(float damage)
    {
        Current_HP -= damage;
        Clamp_Stats();
    }

    public void Heal_HP(float amount)
    {
        Current_HP += amount;
        Clamp_Stats();
    }
}