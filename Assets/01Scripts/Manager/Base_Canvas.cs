using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance;

    [SerializeField]
    private Transform[] Layers;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Base_Manager.ui_Mng.Get_UI("UI_Explain", 2);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Base_Manager.ui_Mng.Get_UI("UI_Inventory", 2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Base_Manager.ui_Mng.Get_UI("UI_Shop", 2);
        }
    }

    public void Set_Layer(UI_Base ui, int index) 
    {
        if (ui == null || Layers == null || Layers.Length <= 0) 
        {
            return;
        }

        if(index < 0 || index >= Layers.Length)
        {
            return;
        }
        ui.transform.SetParent(Layers[index], false);
    }

    public void Get_Text_Pop_Up(string temp, Color color)
    {
        UI_Text_Pop_Up pop_Up = null;
        Base_Manager.pool_Mng.Pooling_OBJ("Text_Pop_Up").Get(obj => 
        {
            pop_Up = obj.GetComponent<UI_Text_Pop_Up>();
            pop_Up.Initalize(temp, color);
            pop_Up.transform.SetParent(this.transform, false);
        });
    }
}
