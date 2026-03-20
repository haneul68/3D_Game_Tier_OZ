using UnityEngine;

public class Enemy_Base : Character
{
    [SerializeField]
    private Vector3 hp_Bar_Offset = new Vector3(0, 2f, 0);

    private Camera cam;

    protected override void Start()
    {
        base.Start();

        if (HP_Bar != null) HP_Bar.gameObject.SetActive(false);
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

        HP_Bar.gameObject.SetActive(is_Fight);

        if (HP_Bar.gameObject.activeSelf)
        {
            HP_Bar.transform.position = cam.WorldToScreenPoint(this.transform.position + hp_Bar_Offset);
        }
    }
}