using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField]
    private string pool_Name;
    [SerializeField]
    private float max_Distance;
    [SerializeField]
    private float move_Speed;
    [SerializeField]
    ParticleSystem effect;

    private float damage;
    private float move_Distance;
    public void Init(float _damage)
    {
        damage = _damage;
        move_Distance = 0f;
        effect.Play();  
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float value = move_Speed * Time.deltaTime;

        transform.position += transform.forward * value;
        move_Distance += value;

        if (move_Distance >= max_Distance)
        {
            Base_Manager.pool_Mng.pool_Dictionary[pool_Name].Return(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Take_Damage(damage);
            Base_Manager.pool_Mng.pool_Dictionary[pool_Name].Return(this.gameObject);
        }
    }
}
