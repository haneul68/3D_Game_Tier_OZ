public interface IDamageable
{
    bool Is_Dead { get; }

    void Take_Damage(float damage);
    void Heal(float amount);
}

