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
    private void Set_Item_Button_Type(Item_Type type)
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

        List<Item_Holder> items_To_Display = Get_Items_To_Display();

        Create_Inventory_Slots(items_To_Display);
        Create_Add_Button();
    }
    private void Create_Inventory_Slots(List<Item_Holder> items_To_Display)
    {
       Base_Manager.inventory_Mng.Get_Inventory_Size((int)current_Type, out int size);

        for (int i = 0; i < size; i++)
        {
            int index = i;
            Base_Manager.pool_Mng.Pooling_OBJ(UI_Pool_Key.INV_ITEM_SLOT).Get(obj =>
            {
                Inventory_Item_Slot slot = obj.GetComponent<Inventory_Item_Slot>();
                slot.transform.SetParent(slot_content, false);
                garbage_Slot.Add(slot.gameObject);
                slot.gameObject.SetActive(true);

                if (index < items_To_Display.Count)
                    slot.Init(items_To_Display[index], DES_Content);
                else
                    slot.Init(null);
            });
        }
    }

    private List<Item_Holder> Get_Items_To_Display()
    {
        List<Item_Holder> items_To_Display = new List<Item_Holder>();
        foreach (var item in Base_Manager.inventory_Mng.inventory_Data.Player_Items)
        {
            var item_Holder = item.Value;

            if (item_Holder.holder.Quantity <= 0 || item_Holder.item_Data.item_Type != current_Type)
                continue;

            int remaining = item_Holder.holder.Quantity;
            int max_Stack = item_Holder.item_Data.max_Stack;

            while (remaining > 0)
            {
                int stack_Count = Mathf.Min(remaining, max_Stack);

                items_To_Display.Add(new Item_Holder
                {
                    item_Data = item_Holder.item_Data,
                    holder = new Holder { Quantity = stack_Count }
                });

                remaining -= stack_Count;
            }
        }
        return items_To_Display;
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
        Base_Manager.inventory_Mng.Inventory_Size[index]++;
        Set_Inventory_Slot();
    }
    #endregion

    #region On_Event
    private void OnItemChanged(Item_Holder item)
    {
        Debug.Log(item.item_Data.name);
        if (item.item_Data.item_Type == current_Type)
        {
            Debug.Log("실행됨");
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

    #region Override
    public override void Close_UI()
    {
        current_Type = Item_Type.None;
        Close_PopUp();
        Return_Prefab_Slot();
        base.Close_UI();
    }
    #endregion
}
