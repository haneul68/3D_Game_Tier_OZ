using TMPro;
using UnityEngine;

public class UI_Text_Pop_Up : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text_Pop_Up_Text;

    public void Initalize(string temp, Color color)
    {
        Text_Pop_Up_Text.color = color;
        Text_Pop_Up_Text.text = temp;

        Invoke(nameof(ReturnToPool), 2f);
    }

    void ReturnToPool()
    {
        Base_Manager.pool_Mng.pool_Dictionary["Text_Pop_Up"].Return(this.gameObject);
    }
}