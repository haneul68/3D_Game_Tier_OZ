using UnityEngine;

public class Enemy_Base : Character
{
    [SerializeField]
    private Vector3 hp_Bar_Offset = new Vector3(0, 3f, 0);

    private Camera cam;

    protected override void Start()
    {
        base.Start();

        Base_Manager.pool_Mng.Pooling_OBJ("Enemy_HP_Bar").Get(obj => 
        {
            obj.SetActive(false);
            HP_Bar = obj.GetComponent<UI_HP_Bar>();
            HP_Bar.transform.SetParent(transform, false);
            HP_Bar.transform.position = transform.position + hp_Bar_Offset;
            HP_Bar.Set_Target(this);
        });

        //if (HP_Bar != null) HP_Bar.gameObject.SetActive(false);
        cam = Camera.main;
    }

    protected override void Update()
    {
        if (HP_Bar == null) return;

        if (Is_Dead)
        {
            HP_Bar.gameObject.SetActive(false);
            return;
        }

        //Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        //bool is_In_Camera_View = GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);

        //HP_Bar.gameObject.SetActive(is_Fight && is_In_Camera_View);

        HP_Bar.gameObject.SetActive(is_Fight);

        if (HP_Bar.gameObject.activeSelf)
        {
            //HP_Bar.transform.position = cam.WorldToScreenPoint(this.transform.position + hp_Bar_Offset);
           

            //HP_Bar.transform.LookAt(Base_Manager.instance.current_Player.transform);
            HP_Bar.transform.LookAt(cam.transform);
        }
    }

    protected override void Die()
    {
        Base_Manager.pool_Mng.pool_Dictionary["Enemy_HP_Bar"].Return(HP_Bar.gameObject);
        base.Die();
    }
}