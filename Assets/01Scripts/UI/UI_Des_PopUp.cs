using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Des_PopUp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI des_Text;

    public void init(Item_Scriptable data) 
    {
        gameObject.SetActive(true);
        string text = string.Format(data.item_Description, data.item_Value);
        des_Text.text = text;
    }
   
}
