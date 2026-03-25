using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Inventory_Item_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] 
    private Image item_Image;
    [SerializeField] 
    private TextMeshProUGUI quantity_Text;

    Item_Scriptable data;
    RectTransform slot_Rect;

   [SerializeField]
    private Transform content;

    private GameObject Popup;
    private GameObject Action_Panal;

    public void Init(Item_Holder item = null) 
    {
        if (item == null)
        {
            data = null;
            item_Image.gameObject.SetActive(false);
            quantity_Text.text = "0";
            return;
        }
        data = item.item_Data;
        Holder holder = item.holder;
        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Atlas(data.item_Name);
        item_Image.SetNativeSize();
        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x/4, slot_Rect.sizeDelta.y / 4);
        quantity_Text.text = holder.Quantity.ToString();

    }

    public void Reset_Data() 
    {
        data = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (Popup != null)
        {
            Base_Manager.pool_Mng.pool_Dictionary["UI_Des"].Return(Popup);
            Popup = null;
        }

        if (Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Count > 0)
        {
            Debug.Log("액션 팝업 활성화 중");
            return;
        }

        Base_Manager.pool_Mng.Pooling_OBJ("Item_Action_Panel").Get(obj =>
        {
            obj.GetComponent<Action_Slot_Panel>().Init(data,this);   
            obj.transform.parent = content;
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 pos = slot_Rect.position;
            pos.x = pos.x + 70f;
            rect.position = pos;

            Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Push(obj);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Count > 0) 
        {
            Debug.Log("액션 팝업 활성화 중");
            return;
        }

        Base_Manager.pool_Mng.Pooling_OBJ("UI_Des").Get(obj => 
        {
            obj.GetComponent<UI_Des_PopUp>().init(data);
            obj.transform.parent = content;
            Popup = obj;
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 pos = slot_Rect.position;
            pos.y = pos.y - 70f;
            rect.position = pos;
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Popup != null)
        {
            Base_Manager.pool_Mng.pool_Dictionary["UI_Des"].Return(Popup);
            Popup = null;
        }
    }
}
