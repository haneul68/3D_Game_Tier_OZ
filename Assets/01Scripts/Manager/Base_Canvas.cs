using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    [SerializeField]
    private Transform[] Layers;

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
}
