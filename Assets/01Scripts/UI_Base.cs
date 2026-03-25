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
        Base_Manager.ui_Mng.ui_Holder.Pop();
        if (Base_Manager.ui_Mng.ui_Holder.Count == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        Destroy(this.gameObject);
    }
}
