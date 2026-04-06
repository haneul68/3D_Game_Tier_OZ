using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Shop_Item_Data
{
    public string item_ID;
    public int total_Amount;
    public int price;
    public Item_Scriptable data;
}

public class UI_Shop : UI_Base
{
    [SerializeField]
    private UI_Inventory ui_Inventory;

    private Shop_Item_Data[] shop_Items;

    #region Slot
    private List<GameObject> garbage_Slot = new List<GameObject>();

    [SerializeField]
    private Transform slot_content;
    #endregion

    [SerializeField]
    private Transform DES_Content;

    [SerializeField]
    private TextMeshProUGUI gold_Text;

    #region 아이템 분류
    private Item_Type current_Type = Item_Type.None;

    [SerializeField]
    private Button[] item_Type_Buttons = new Button[3];
    private Vector2[] origin_Pos;
    private Vector3[] origin_Scale;
    private RectTransform[] button_Rects;
    private Coroutine select_Coroutine;

    [SerializeField] private float button_Anim_Duration = 0.2f;
    [SerializeField] private Vector2 button_Move_Offset = new Vector2(0, 7f);
    [SerializeField] private float button_Scale_Multiplier = 1.1f;
    #endregion
    private void Awake()
    {
        Item_Type_Button_Init();
    }

    public void Init_Shop_Data(Dictionary<string, Shop_Item_Data> shop_Item_Datas, Shop_Interact_OBJ current_Shop)
    {
        if (shop_Item_Datas == null || shop_Item_Datas.Count == 0 || current_Shop == null) return;
        Base_Manager.shop_Mng.current_Shop = current_Shop;
        shop_Items = shop_Item_Datas.Values.ToArray();
        Set_Item_Button_Type((Item_Type)0);
        Set_Gold_Text();
        Base_Manager.shop_Mng.OnShopItemChanged += OnShopItemChanged;
    }

    private void OnDisable()
    {
        Base_Manager.shop_Mng.OnShopItemChanged -= OnShopItemChanged;
    }

    #region Draw_Slot
    private void Set_Inventory_Slot()
    {
        Return_Prefab_Slot();

        Create_Inventory_Slots();
    }

    private void Create_Inventory_Slots()
    {
        for (int i = 0; i < shop_Items.Length; i++)
        {
            var item_Data = shop_Items[i].data;
            if (item_Data == null) continue;

            if (shop_Items[i].data.item_Type != current_Type) continue;

            Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.SHOP_ITEM_SLOT).Get(obj =>
            {
                Shop_Item_Slot uiSlot = obj.GetComponent<Shop_Item_Slot>();
                uiSlot.transform.SetParent(slot_content, false);
                garbage_Slot.Add(uiSlot.gameObject);
                uiSlot.gameObject.SetActive(true);
                uiSlot.Init(shop_Items[i], DES_Content, i);
            });
        }
    }

    private void Return_Prefab_Slot()
    {
        if (garbage_Slot.Count > 0)
        {
            for (int i = 0; i < garbage_Slot.Count; i++)
            {
                Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.SHOP_ITEM_SLOT].Return(garbage_Slot[i]);
            }
            garbage_Slot.Clear();
        }
    }
    #endregion
    private void Close_PopUp()
    {
        if (Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Count > 0)
        {
            GameObject obj = Base_Manager.shop_Mng.Shop_Acrtion_Panal_Holder.Pop();
            Debug.Log(obj);
            obj.GetComponent<Action_Slot_Panel>().Close_Panel();
        }
    }
    public void Reset_UI()
    {
        current_Type = Item_Type.None;
        Close_PopUp();
        Return_Prefab_Slot();
    }

    public override void Close_UI()
    {
        Base_Manager.shop_Mng.current_Shop.Is_Interactable = false;
        Reset_UI();
        base.Close_UI();
    }
    #region Item_Type_Button
    private void Item_Type_Button_Init()
    {
        origin_Pos = new Vector2[item_Type_Buttons.Length];
        origin_Scale = new Vector3[item_Type_Buttons.Length];

        button_Rects = new RectTransform[item_Type_Buttons.Length];

        for (int i = 0; i < item_Type_Buttons.Length; i++)
        {
            button_Rects[i] = item_Type_Buttons[i].GetComponent<RectTransform>();
            origin_Pos[i] = button_Rects[i].anchoredPosition;
            origin_Scale[i] = button_Rects[i].localScale;

            int index = i;
            item_Type_Buttons[i].onClick.RemoveAllListeners();
            item_Type_Buttons[i].onClick.AddListener(() => Set_Item_Button_Type((Item_Type)index));
        }
    }
    private void Set_Item_Button_Type(Item_Type type)
    {
        if (current_Type == type)
        {
            return;
        }
        current_Type = type;

        ui_Inventory.Set_Item_Button_Type(current_Type);

        Set_Inventory_Slot();

        Close_PopUp();

        if (select_Coroutine != null)
        {
            StopCoroutine(select_Coroutine);
            select_Coroutine = null;
        }

        select_Coroutine = StartCoroutine(Select_Button());
    }

    private IEnumerator Select_Button()
    {
        float time = 0f;

        int Index = (int)current_Type;

        for (int i = 0; i < item_Type_Buttons.Length; i++)
        {
            RectTransform r = item_Type_Buttons[i].GetComponent<RectTransform>();
            r.anchoredPosition = origin_Pos[i];
            r.localScale = origin_Scale[i];
        }

        RectTransform rect = item_Type_Buttons[Index].GetComponent<RectTransform>();

        Vector2 startPos = origin_Pos[Index];
        Vector2 endPos = origin_Pos[Index] + button_Move_Offset;

        Vector3 startScale = origin_Scale[Index];
        Vector3 endScale = origin_Scale[Index] * button_Scale_Multiplier;

        while (time < button_Anim_Duration)
        {
            time += Time.deltaTime;
            float t = time / button_Anim_Duration;

            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            rect.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        rect.anchoredPosition = endPos;
        rect.localScale = endScale;
    }
    #endregion

    #region On_Event

    private void OnShopItemChanged(Item_Scriptable item)
    {
        if (item.item_Type == current_Type)
        {
            Set_Inventory_Slot();
            Set_Gold_Text();
        }
    }
    #endregion

    private void Set_Gold_Text() 
    {
        if (gold_Text == null) return;
        gold_Text.text = Base_Manager.data_Mng.Gold.ToString();
    }
}
