using System.Collections;
using UnityEngine;

public class Player : Character
{
    private Transform cam;

    protected CharacterController controller;

    #region INPUT_VARIABLE
    private float input_H;
    private float input_V;
    private bool input_run;
    [SerializeField]
    private bool input_Attack;
    [SerializeField]
    private bool is_Attacking;

    private Vector3 input_Dir;
    #endregion

    #region Attack
    [SerializeField]
    int Attack_Combo = 0;
    private Coroutine isAttack_Coroutine = null;
    [SerializeField]
    private Transform T_Combo_Effect_Position;
    #endregion
    protected override void Start()
    {
        base.Start();
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        Get_Input();

        Handle_Move_Mode();

        Handle_Animation_Move(Get_Input_Dir(), input_run);

        Handle_Jump();

        Handle_Attack();

        Move();

    }
    #region INPUT_SYSTEM
    private void Get_Input()
    {
        input_H = Input.GetAxisRaw("Horizontal");
        input_V = Input.GetAxisRaw("Vertical");
        input_run = Input.GetKey(KeyCode.LeftShift);
        input_Attack = Input.GetMouseButtonDown(0);
    }
    private void Handle_Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Try_Jump();
        }
    }
    #endregion

    #region MOVE_SPEED
    float Get_Move_Speed()
    {
        float base_Speed = stats.Get_Move_Speed;

        float h = input_H;
        float v = input_V;
        bool isRun = input_run;

        Camera_Mode current_Mode = Base_Manager.game_Mng.current_Mode;

        float run_Value = 3.0f;

        if (current_Mode == Camera_Mode.TPS || current_Mode == Camera_Mode.FPS)
        {
            bool forward = v > 0;
            bool backward = v < 0;
            bool side = h != 0;

            float back_Value = 0.5f;
            float side_Value = 0.8f;
           

            if (backward)
                return base_Speed * back_Value;

            if (side && !forward)
                return base_Speed * side_Value;

            if (forward && side)
                return base_Speed * side_Value;

            if (forward && isRun)
                return base_Speed * run_Value;

            if (forward)
                return base_Speed;
        }
        else if (current_Mode == Camera_Mode.RPG) 
        {
            if (isRun) 
            {
                return base_Speed * run_Value;
            }
            else
            {
                return base_Speed;
            }
        }

        return 0;
    }
    #endregion

    #region MOVE_MODE
    private void Handle_Move_Mode()
    {
        if (Base_Manager.game_Mng.current_Mode == Camera_Mode.TPS || Base_Manager.game_Mng.current_Mode == Camera_Mode.FPS)
        {
            Handle_TPS_Mode();
        }
        else if (Base_Manager.game_Mng.current_Mode == Camera_Mode.RPG)
        {
            Handle_RPG_Mode();
        }
    }
    private void Handle_TPS_Mode()
    {
        TPS_FPS_Move();
    }
    private void Handle_RPG_Mode()
    {
        RPG_Move();
    }
    private Vector3 Get_Input_Dir()
    {
        Vector3 input_Dir = new Vector3(input_H, 0, input_V);

        if (input_Dir.sqrMagnitude > 1f)
            input_Dir.Normalize();

        return input_Dir;
    }
    private void RPG_Move()
    {
        Vector3 _move_Dir = Get_Input_Dir();

        if (_move_Dir.sqrMagnitude > 0.01f)
        {
            move_Dir = _move_Dir;

            float rotate_Speed = 10f;
            Quaternion target_Rot = Quaternion.LookRotation(_move_Dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_Rot, rotate_Speed * Time.deltaTime);
        }
        else
        {
            move_Dir = Vector3.zero;
        }
    }
    private void TPS_FPS_Move()
    {
        Vector3 input_Dir = Get_Input_Dir();

        Vector3 cam_Forward = cam.forward;
        Vector3 cam_Right = cam.right;

        cam_Forward.y = 0;
        cam_Right.y = 0;

        cam_Forward.Normalize();
        cam_Right.Normalize();

        move_Dir = cam_Forward * input_Dir.z + cam_Right * input_Dir.x;

        if (move_Dir.magnitude > 1)
            move_Dir.Normalize();
    }
    #endregion

    #region Override
    protected override void Move()
    {
        if(is_Attacking == true) return;

        float speed = Get_Move_Speed();

        is_Grounded = controller.isGrounded;

        if (is_Grounded && vertical_Velocity < 0)
        {
            vertical_Velocity = -2f;
            is_Jumping = false;
        }

        Vector3 h_Move = move_Dir * speed;

        Vector3 velocity = h_Move;
        velocity.y = vertical_Velocity;

        controller.Move(velocity * Time.deltaTime);

        if (vertical_Velocity < 0)
        {
            vertical_Velocity += gravity * 2f * Time.deltaTime;
        }
        else
        {
            vertical_Velocity += gravity * Time.deltaTime;
        }
    }
    #endregion

    #region ANIMATION
    private void Handle_Animation_Move(Vector3 move_Input, bool is_Running)
    {
        if (move_Input.sqrMagnitude < 0.01f)
        {
            Change_Animation(Character_Animetion_State.isIdle);
            return;
        }

        if (Base_Manager.game_Mng.current_Mode == Camera_Mode.RPG)
        {
            if (is_Running)
                Change_Animation(Character_Animetion_State.isRun);
            else
                Change_Animation(Character_Animetion_State.isWalk_F);

            return;
        }

        float h = move_Input.x;
        float v = move_Input.z;

        if (v > 0 && h == 0 && is_Running)
        {
            Change_Animation(Character_Animetion_State.isRun);
            return;
        }
        if (v > 0)
        {
            Change_Animation(Character_Animetion_State.isWalk_F);
            return;
        }
        if (v < 0)
        {
            Change_Animation(Character_Animetion_State.isWalk_B);
            return;
        }
        if (h < 0)
        {
            Change_Animation(Character_Animetion_State.isWalk_L);
            return;
        }
        if (h > 0)
        {
            Change_Animation(Character_Animetion_State.isWalk_R);
        }
    }
  
    private void Handle_Attack() 
    {
        if (is_Jumping == true) return;

        if(is_Attacking == true) return;

        if (input_Attack == true) 
        {
            is_Attacking = true;
           
            switch (Attack_Combo) 
            {
                case 0:
                    Change_Animation(Character_Animetion_State.isAttack_1);
                    Attack_Combo++;
                    break;
                case 1:
                    Change_Animation(Character_Animetion_State.isAttack_2);
                    Attack_Combo++;
                    break;
                case 2:
                    Change_Animation(Character_Animetion_State.isAttack_3);
                    break;
            }
            if (isAttack_Coroutine != null)
            {
                StopCoroutine(isAttack_Coroutine);
                isAttack_Coroutine = null;
            }
        }
    }
    private void OnAnimatorMove()
    {
        if (is_Attacking)
        {
            controller.Move(animator.deltaPosition);
        }
    }

    public void Start_Attack_Combo_Timmer() 
    {
        is_Attacking = false;
        if (isAttack_Coroutine != null)
        {
            StopCoroutine(isAttack_Coroutine);
            isAttack_Coroutine = null;
        }
        isAttack_Coroutine = StartCoroutine("Attack_Combo_Timmer");
    }

    public void End_Combo_Attack_Event() 
    {
        Attack_Combo = 0;
        is_Attacking = false;
    }
    private IEnumerator Attack_Combo_Timmer() 
    {
        float default_Time = 2.0f;
        float timmer = 0f;

        while (default_Time >= timmer) 
        {
            timmer += Time.deltaTime;
            yield return null;
        }
        Attack_Combo = 0;
        is_Attacking = false;
    }

    private void Succes_Combo() 
    {
        Base_Manager.pool_Mng.Pooling_OBJ("3_Combo_Slash").Get(obj => 
        {
            float damage = stats.Get_Attack_Power;
            obj.GetComponent<Projectiles>().Init(damage);
            Vector3 pos = T_Combo_Effect_Position.position;
            pos.y = 0f;
            obj.transform.position = pos;
            obj.transform.rotation = T_Combo_Effect_Position.rotation;
        });
    }
    #endregion
}