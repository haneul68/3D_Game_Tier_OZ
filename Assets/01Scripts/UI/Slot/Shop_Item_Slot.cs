using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Shop_Item_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int Slot_Index { get; private set; }

    [SerializeField] 
    private Image item_Image;
    [SerializeField] 
    private TextMeshProUGUI total_Amount_Text;
    [SerializeField]
    private TextMeshProUGUI price_Text;

    Shop_Item_Data data;
    RectTransform slot_Rect;

    private Transform content;

    private GameObject Popup;

    public int item_stack = 0;

    public void Init(Shop_Item_Data item, Transform popup_Content = null, int slotIndex = -1)
    {
        Slot_Index = slotIndex;

        if (item.data == null)
        {
            return;
        }

        if (popup_Content != null)
            content = popup_Content;

        data = item;


        item_Image.gameObject.SetActive(true);
        item_Image.sprite = Utils.Get_Atlas(item.item_ID);
        item_Image.SetNativeSize();

        slot_Rect = item_Image.GetComponent<RectTransform>();
        slot_Rect.sizeDelta = new Vector2(slot_Rect.sizeDelta.x / 4, slot_Rect.sizeDelta.y / 4);

        total_Amount_Text.text = item.total_Amount.ToString();
        price_Text.text = item.price.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data.data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (Popup != null)
        {
            Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.UI_DES_POPUP].Return(Popup);
            Popup = null;
        }

        if (Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Count > 0)
        {
            Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Pop().GetComponent<Shop_Action_Slot_Panel>().Close_Panel();
            Debug.Log($"기존 액션 팝업 종료{Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Count}");
        }

        Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.SHOP_ITEM_ACTION_PANEL).Get(obj =>
        {
            obj.GetComponent<Shop_Action_Slot_Panel>().Init(data, this);
            obj.transform.SetParent(content, false);
            RectTransform rect = obj.GetComponent<RectTransform>();

            Vector2 pos = slot_Rect.position;
            pos.x = pos.x + 70f;
            rect.position = pos;

            Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Push(obj);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data.data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        if (Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Count > 0) 
        {
            Debug.Log("액션 팝업 활성화 중");
            return;
        }

        Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.UI_DES_POPUP).Get(obj =>
        {
            obj.GetComponent<UI_Des_PopUp>().init(data.data);
            obj.transform.SetParent(content, false);
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
            Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.UI_DES_POPUP].Return(Popup);
            Popup = null;
        }
    }
}
