using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Weapon_info 
{
    public Weapon_Type type;
    public float ATK;
}

public class Weapon : Item_Base
{
    [SerializeField]
    Weapon_info weapon_Info;
    Coroutine weapon_Coroutine = null;

    [SerializeField]
    private BoxCollider attack_Collider = null;

    private List<IDamageable> hit_Targets = new List<IDamageable>();

    private IAttacker attacker;
    public void Start_Attack(IAttacker _attackr) 
    {
        if(_attackr == null) return;

        attacker = _attackr;
    }

    public void On_Collider() 
    {
        hit_Targets.Clear();
        attack_Collider.enabled = true;
    }
    public void Off_Collider() 
    {
        attack_Collider.enabled = false;
    }

    //IEnumerator Melee_Attack() 
    //{
    //    hit_Targets.Clear();
    //    yield return new WaitForSeconds(0.15f);
    //    attack_Collider.enabled = true;

    //    yield return new WaitForSeconds(0.7f);
    //    attack_Collider.enabled = false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null) return;

        if (target == attacker) return;

        if (hit_Targets.Contains(target)) return;

        Debug.Log(target);

        if (attacker is Character character)
        {
            float value = character.character_Stats.Get_Attack_Power;
            target.Take_Damage(value);
        }

        hit_Targets.Add(target);
    }
}
