using UnityEngine;

public class Game_Manager
{
    public Camera_Mode current_Mode = Camera_Mode.TPS;
    public Camera_Mode ui_Selected_Mode = Camera_Mode.TPS;  
    public bool[] game_Mode = new bool[2];
    public bool is_Camera_Mode_Locked = false;

    public bool is_RPG_Disabled = false;

    public void Set_UI_Selected_Mode(Camera_Mode mode)
    {
        if (mode == Camera_Mode.RPG && is_RPG_Disabled)
        {
            ui_Selected_Mode = Camera_Mode.TPS;
            current_Mode = Camera_Mode.TPS;
        }
        else
        {
            ui_Selected_Mode = mode;
            if (!is_Camera_Mode_Locked)
                current_Mode = mode;
        }
    }

    public void Restore_Mode_From_UI()
    {
        if (is_RPG_Disabled && ui_Selected_Mode == Camera_Mode.RPG)
            current_Mode = Camera_Mode.TPS;
        else
            current_Mode = ui_Selected_Mode;
    }
}
