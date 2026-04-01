using UnityEngine;

public class UI_Base : MonoBehaviour 
{
    private void Start()
    {
        Init();
    }
    protected virtual void Init() 
    {
    }
    public virtual void Close_UI()
    {
        Base_Manager.ui_Mng.Close_Top_UI();
    }
}
