using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager
{
    public Stack<UI_Base> ui_Holder = new Stack<UI_Base>();
    private Base_Canvas base_Canvas;

    private Dictionary<string, UI_Base> ui_Cache = new Dictionary<string, UI_Base>();

    public void Init(Base_Canvas _base_Canvas) 
    {
        if (_base_Canvas == null) return;

        base_Canvas = _base_Canvas;
    }

    public UI_Base Get_UI(string path, int layer_Index)
    {
        if (string.IsNullOrEmpty(path)) return null;

        if (ui_Holder.Count > 0 && ui_Holder.Peek().name == path) return null;

        UI_Base ui;

        if (ui_Cache.ContainsKey(path))
        {
            ui = ui_Cache[path];
            ui.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = Base_Manager.instance.Get_Prefab_OBJ("UI/" + path);
            go.name = path;
            ui = go.GetComponent<UI_Base>();

            base_Canvas.Set_Layer(ui, layer_Index);

            ui_Cache[path] = ui;
        }

        ui.transform.SetAsLastSibling();

        ui_Holder.Push(ui);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        return ui;
    }

    public void Close_Top_UI()
    {
        if (ui_Holder.Count == 0) return;

        UI_Base ui = ui_Holder.Pop();
        if (ui != null)
        {
            ui.gameObject.SetActive(false);
        }

        Update_Cursor_State();
    }

    public void Close_All_UI()
    {
        if (ui_Holder.Count == 0) return;

        Stack<UI_Base> tempStack = new Stack<UI_Base>();

        while (ui_Holder.Count > 0)
        {
            UI_Base ui = ui_Holder.Pop();
            if (ui != null)
            {
                ui.gameObject.SetActive(false);
                tempStack.Push(ui);
            }
        }

        Update_Cursor_State();
    }

    private void Update_Cursor_State()
    {
        if (ui_Holder.Count == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
