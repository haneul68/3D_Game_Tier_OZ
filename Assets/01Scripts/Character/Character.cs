using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable, IAttacker
{
    #region Jump
    protected float gravity = -9.81f;
    [SerializeField]
    protected float jump_Force = 3f;

    protected float vertical_Velocity;
    protected bool is_Grounded;
    protected bool is_Jumping = false;
    #endregion

    #region Attack
    public bool is_Attacking { get; set; }
    [SerializeField]
    private Weapon current_Weapon; // 임시코드 
    #endregion

    [SerializeField]
    protected Character_Stats stats = new Character_Stats();

    protected Vector3 move_Dir;
    #region State
    protected Animator animator;
    public Character_Stats character_Stats => stats;

    protected Character_Animetion_State current_State;

    public bool is_Fight = false;
    private Coroutine fight_Coroutine;
    [SerializeField]
    private float fight_Duration;
    #endregion
    #region HP_UI
    [SerializeField]
    public UI_HP_Bar HP_Bar;

    public event Action<float, float> On_HP_Change_Event;
    #endregion
    public bool Is_Dead => stats.Get_Current_HP <= 0;

    private void Awake()
    {
        HP_Bar.Set_Target(this);
    }

    protected virtual void Start()
    {
        stats.Init();
        animator = GetComponent<Animator>();
        On_HP_Change_Event?.Invoke(stats.Get_Current_HP,stats.Get_Max_HP);
    }

    protected virtual void Update()
    {
      
    }
    #region ANNIMETION
    public void Change_Animation(Character_Animetion_State new_State)
    {
        if (animator == null)
        {
            Debug.LogWarning("애니메이터 없음");
            return;
        }
        if (current_State == new_State) return;

        current_State = new_State;

        Reset_Anim_Bool();

        switch (current_State)
        {
            case Character_Animetion_State.isJump:
                animator.SetTrigger(AnimatorHash.isJump);
                return;
            case Character_Animetion_State.isAttack_1:
                animator.SetTrigger(AnimatorHash.isAttack1);
                return;
            case Character_Animetion_State.isAttack_2:
                animator.SetTrigger(AnimatorHash.isAttack2);
                return;
            case Character_Animetion_State.isAttack_3:
                animator.SetTrigger(AnimatorHash.isAttack3);
                return;
            case Character_Animetion_State.isDie:
                animator.SetTrigger(AnimatorHash.isDie);
                return;
            case Character_Animetion_State.isHit:
                animator.SetTrigger(AnimatorHash.isHit);
                current_State = Character_Animetion_State.None;
                return;


            case Character_Animetion_State.isIdle:
                animator.SetBool(AnimatorHash.isIdle, true);
                return;
            case Character_Animetion_State.isWalk_F:
                animator.SetBool(AnimatorHash.isWalk_F, true);
                return;
            case Character_Animetion_State.isWalk_L:
                animator.SetBool(AnimatorHash.isWalk_L, true);
                return;
            case Character_Animetion_State.isWalk_R:
                animator.SetBool(AnimatorHash.isWalk_R, true);
                return;
            case Character_Animetion_State.isWalk_B:
                animator.SetBool(AnimatorHash.isWalk_B, true);
                return;
            case Character_Animetion_State.isRun:
                animator.SetBool(AnimatorHash.isRun, true);
                return;
        }
    }

    void Reset_Anim_Bool()
    {
        animator.SetBool(AnimatorHash.isIdle, false);
        animator.SetBool(AnimatorHash.isWalk_F, false);
        animator.SetBool(AnimatorHash.isWalk_L, false);
        animator.SetBool(AnimatorHash.isWalk_R, false);
        animator.SetBool(AnimatorHash.isWalk_B, false);
        animator.SetBool(AnimatorHash.isRun, false);
    }
    #endregion

    #region JUMP
    protected void Jump()
    {
        if (is_Jumping) return;
        vertical_Velocity = Mathf.Sqrt(jump_Force * -1f * gravity);
        is_Jumping = true;
    }

    protected void Try_Jump()
    {
        if (!is_Grounded) return;
        Change_Animation(Character_Animetion_State.isJump);
    }
    #endregion
    protected virtual void Move()
    {
        //float speed = stats.Get_Move_Speed;

        //controller.Move(move_Dir * speed * Time.deltaTime);
    }

    #region HP 관련
    public virtual void Heal(float amount)
    {
        if(Is_Dead) return;

        stats.Heal_HP(amount);

        Change_HP();
    }

    public virtual void Take_Damage(float damage)
    {
        if (Is_Dead) return;

        Start_Fight();

        stats.Take_HP_Damage(damage);

        if (!Is_Dead) 
        {
            Change_Animation(Character_Animetion_State.isHit);
            is_Attacking = false;
        }

        Change_HP();

        if (Is_Dead)
        {
            Die();
        }
    }

    private void Change_HP() 
    {
        float max_HP = stats.Get_Max_HP;
        float current_HP = stats.Get_Current_HP;

        On_HP_Change_Event?.Invoke(current_HP, max_HP);
    }

    protected virtual void Die()
    {
        Debug.Log($"{name}가 죽었습니다.");
        Change_Animation(Character_Animetion_State.isDie);
        StartCoroutine(Destroy_Delay(2f));
    }

    private IEnumerator Destroy_Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    #endregion

    #region Attack
    protected virtual void Attack() 
    {
        Start_Fight();
        current_Weapon.Start_Attack(this);
    }

    public void On_Weapon_Collider() 
    {
        if (current_Weapon == null) return;
        current_Weapon.On_Collider();
    }
    public void Off_Weapon_Collider()
    {
        if (current_Weapon == null) return;
        current_Weapon.Off_Collider();
    }
    #endregion

    public void Start_Fight()
    {
        is_Fight = true;

        if (fight_Coroutine != null) 
        {
            StopCoroutine(fight_Coroutine);
            fight_Coroutine = null;
        }
        
        fight_Coroutine = StartCoroutine(Fight_Cooldown());
    }

    private IEnumerator Fight_Cooldown()
    {
        float timer = 0f;

        while (timer < fight_Duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        is_Fight = false;
        fight_Coroutine = null;
    }
}