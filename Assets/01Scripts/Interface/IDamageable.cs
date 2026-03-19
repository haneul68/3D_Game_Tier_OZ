using System;
public interface IDamageable
{
    bool Is_Dead { get; }

    void Take_Damage(float damage);
    void Heal(float amount);

    event Action<float, float> On_HP_Change_Event;
}

