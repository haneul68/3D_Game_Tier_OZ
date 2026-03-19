using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    #region Jump
    protected float gravity = -9.81f;
    [SerializeField]
    protected float jump_Force = 3f;

    protected float vertical_Velocity;
    protected bool is_Grounded;
    protected bool is_Jumping = false;
    #endregion

    [SerializeField]
    protected Character_Stats stats = new Character_Stats();

    protected Vector3 move_Dir;


    protected Animator animator;

    protected Character_Animetion_State current_State;

    public bool Is_Dead => stats.Get_Current_HP <= 0;

    protected virtual void Start()
    {
        stats.Init();
        animator = GetComponent<Animator>();
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

        string temp = current_State.ToString();

        Reset_Anim_Bool();

        switch (current_State)
        {
            case Character_Animetion_State.isJump:
                animator.SetTrigger(temp);
                return;
            case Character_Animetion_State.isAttack_1:
                animator.SetTrigger(temp);
                return;
            case Character_Animetion_State.isAttack_2:
                animator.SetTrigger(temp);
                return;
            case Character_Animetion_State.isAttack_3:
                animator.SetTrigger(temp);
                return;
        }

        animator.SetBool(temp, true);
    }

    void Reset_Anim_Bool()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalk_F", false);
        animator.SetBool("isWalk_L", false);
        animator.SetBool("isWalk_R", false);
        animator.SetBool("isWalk_B", false);
        animator.SetBool("isRun", false);
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
        stats.Heal_HP(amount);
    }

    public virtual void Take_Damage(float damage)
    {
        if (Is_Dead) return;

        stats.Take_HP_Damage(damage);

        if (Is_Dead)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log($"{name}가 죽었습니다.");
        Destroy(gameObject);
    }
    #endregion

    #region Attack
    protected virtual void Attack() 
    {
        
    }
    #endregion
}