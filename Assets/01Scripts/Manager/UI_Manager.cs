using System.Collections.Generic;
using UnityEngine;

public class UI_Manager
{
    public Stack<UI_Base> ui_Holder = new Stack<UI_Base>();
    private Base_Canvas base_Canvas;

    public void Init(Base_Canvas _base_Canvas) 
    {
        if (_base_Canvas == null) return;

        base_Canvas = _base_Canvas;
    }

    public void Get_UI(string path, int layer_Index)
    {
        if(ui_Holder == null) return ;
        if (Check_UI_Enable(path)) return;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;
        GameObject go = Base_Manager.instance.Get_Prefab_OBJ("UI/" + path);
        go.name = path;
        UI_Base ui = go.GetComponent<UI_Base>();
        base_Canvas.Set_Layer(ui, layer_Index);
        ui_Holder.Push(ui); 
    }

    private bool Check_UI_Enable(string path) 
    {
        if (ui_Holder.Count > 0) 
        {
            if (ui_Holder.Peek().name == path)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        return false;
    }
}
