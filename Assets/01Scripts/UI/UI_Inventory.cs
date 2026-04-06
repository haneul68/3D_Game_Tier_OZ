using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    #region Slot
    private List<GameObject> garbage_Slot = new List<GameObject>();
    private GameObject garbage_Add_Button = null;

    [SerializeField]
    private Transform slot_content;
    [SerializeField]
    private readonly int default_Slot_Row = 11; 

    [SerializeField]  
    private Item_Slot_Type Inventory_Item_Slot_Type = Item_Slot_Type.None;
    #endregion

    [SerializeField]
    private Transform DES_Content;

    #region 아이템 분류
    private Item_Type current_Type = Item_Type.None;

    [SerializeField]
    private Button[] item_Type_Buttons = new Button[3];
    private Vector2[] origin_Pos;
    private Vector3[] origin_Scale;
    private RectTransform[] button_Rects;
    private Coroutine select_Coroutine;

    [SerializeField] private float button_Anim_Duration = 0.2f;
    [SerializeField] private Vector2 button_Move_Offset = new Vector2(7f, 0);
    [SerializeField] private float button_Scale_Multiplier = 1.1f;
    #endregion


    private void Awake()
    {
        Item_Type_Button_Init();
    }

    private void OnEnable()
    {
        Set_Item_Button_Type((Item_Type)0);
        Base_Manager.inventory_Mng.inventory_Logic.OnItemChanged += OnItemChanged;
    }
    private void OnDisable()
    {
        if (Base_Manager.inventory_Mng.inventory_Logic != null)
            Base_Manager.inventory_Mng.inventory_Logic.OnItemChanged -= OnItemChanged;
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
    public void Set_Item_Button_Type(Item_Type type)
    {
        if (current_Type == type)
        {
            return;
        }
        current_Type = type;
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

    #region Draw_Slot
    private void Set_Inventory_Slot()
    {
        Return_Prefab_Slot();

        Create_Inventory_Slots();
        Create_Add_Button();
    }
   
    private void Create_Inventory_Slots()
    {
        var slots = Base_Manager.inventory_Mng.inventory_Data.Inventory_Slots[current_Type];

        for (int i = 0; i < slots.Count; i++)
        {
            int index = i;

            Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.INV_ITEM_SLOT).Get(obj =>
            {
                Inventory_Item_Slot uiSlot = obj.GetComponent<Inventory_Item_Slot>();
                uiSlot.transform.SetParent(slot_content, false);
                garbage_Slot.Add(uiSlot.gameObject);
                uiSlot.gameObject.SetActive(true);

                var dataSlot = slots[index];

                if (!dataSlot.IsEmpty)
                {
                    uiSlot.Init(dataSlot.item, dataSlot.quantity, DES_Content, index, Inventory_Item_Slot_Type); 
                }
                else
                {
                    uiSlot.Init(null, 0, DES_Content, index, Inventory_Item_Slot_Type);
                }
            });
        }
    }
  
    private void Create_Add_Button()
    {
        Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.INV_ADD_BUTTON).Get(obj =>
        {
            Button button = obj.GetComponent<Button>();
            button.transform.SetParent(slot_content, false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Expand_Inventory();
                Close_PopUp();
            });
            garbage_Add_Button = button.gameObject;
        });
    }
    private void Return_Prefab_Slot()
    {
        if (garbage_Slot.Count > 0)
        {
            for (int i = 0; i < garbage_Slot.Count; i++)
            {
                Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.INV_ITEM_SLOT].Return(garbage_Slot[i]);
            }
            if (garbage_Add_Button != null)
            {
                Base_Manager.pool_Mng.pool_Dictionary[UI_Pool_Key.INV_ADD_BUTTON].Return(garbage_Add_Button);
                garbage_Add_Button = null;
            }
            garbage_Slot.Clear();
        }
    }
    #endregion

    #region Expand_Button
    public void Expand_Inventory()
    {
        int index = (int)current_Type;
      
        var slots = Base_Manager.inventory_Mng.inventory_Data.Inventory_Slots[current_Type];
        int defaut_Size = Base_Manager.inventory_Mng.inventory_Data.default_Slot_Size;
        for (int i = 0; i < defaut_Size; i++) 
        {
            slots.Add(new Inventory_Slot());
        }
        Create_Additional_Slots(defaut_Size);
    }
    private void Create_Additional_Slots(int count)
    {
        var slots = Base_Manager.inventory_Mng.inventory_Data.Inventory_Slots[current_Type];
        int startIndex = slots.Count - count;

        for (int i = startIndex; i < slots.Count; i++)
        {
            int index = i;
            Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.INV_ITEM_SLOT).Get(obj =>
            {
                Inventory_Item_Slot ui_Slot = obj.GetComponent<Inventory_Item_Slot>();
                ui_Slot.transform.SetParent(slot_content, false);
                garbage_Slot.Add(ui_Slot.gameObject);
                ui_Slot.gameObject.SetActive(true);

                var data_Slot = slots[index];
                if (!data_Slot.IsEmpty)
                {
                    ui_Slot.Init(data_Slot.item, data_Slot.quantity, DES_Content, index, Inventory_Item_Slot_Type);
                }
                else
                {
                    ui_Slot.Init(null, 0, DES_Content, index, Inventory_Item_Slot_Type);
                }
            });
        }

        if (garbage_Add_Button != null)
            garbage_Add_Button.transform.SetAsLastSibling();
    }
    #endregion

    #region On_Event

    private void OnItemChanged(Item_Scriptable item)
    {
        if (item.item_Type == current_Type)
        {
            Set_Inventory_Slot();
        }
    }
    #endregion
    private void Close_PopUp() 
    {
        if (Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Count > 0)
        {
            GameObject obj = Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Pop();
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

    #region Override
    public override void Close_UI()
    {
        Reset_UI();
        base.Close_UI();
    }
    #endregion
}
