using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    [SerializeField]
    private Toggle TPS_Mode;
    [SerializeField]
    private Toggle RPG_Mode;

    protected override void Init() 
    {
        Init_Toggle();

        TPS_Mode.onValueChanged.AddListener(OnTPS);
        RPG_Mode.onValueChanged.AddListener(OnRPS);
    }

    private void OnTPS(bool isOn)
    {
        if (!isOn) return;

        Base_Manager.game_Mng.current_Mode = Camera_Mode.TPS;
        Init_Toggle();
    }

    private void OnRPS(bool isOn)
    {
        if (!isOn) return;

        Base_Manager.game_Mng.current_Mode = Camera_Mode.RPG;
        Init_Toggle();
    }

    private void Init_Toggle() 
    {
        switch (Base_Manager.game_Mng.current_Mode)
        {
            case Camera_Mode.TPS:
                TPS_Mode.SetIsOnWithoutNotify(true);
                RPG_Mode.SetIsOnWithoutNotify(false);
                break;

            case Camera_Mode.RPG:
                TPS_Mode.SetIsOnWithoutNotify(false);
                RPG_Mode.SetIsOnWithoutNotify(true);
                break;
            case Camera_Mode.FPS:
                TPS_Mode.SetIsOnWithoutNotify(true);
                RPG_Mode.SetIsOnWithoutNotify(false);
                break;
        }
    }
}
