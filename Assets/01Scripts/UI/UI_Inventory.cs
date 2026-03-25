using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    private List<GameObject> garvage_Slot = new List<GameObject>();

    [SerializeField]
    private Inventory_Item_Slot slot_Prefab;
    [SerializeField]
    private Button expand_Slot_Button;
    [SerializeField]
    private Transform content;

    private Item_Type current_Type;
    [SerializeField]
    private Button[] item_Type_Buttons = new Button[3];
    private Vector2[] origin_Pos;
    private Vector3[] origin_Scale;

    private Coroutine select_Coroutine;

    void Start()
    {
        origin_Pos = new Vector2[item_Type_Buttons.Length];
        origin_Scale = new Vector3[item_Type_Buttons.Length];

        for (int i = 0; i < item_Type_Buttons.Length; i++)
        {
            RectTransform r = item_Type_Buttons[i].GetComponent<RectTransform>();
            origin_Pos[i] = r.anchoredPosition;
            origin_Scale[i] = r.localScale;

            int index = i;
            item_Type_Buttons[i].onClick.RemoveAllListeners();
            item_Type_Buttons[i].onClick.AddListener(() => Set_Item_Button_Type((Item_Type)index));
        }

        Set_Item_Button_Type((Item_Type)0);
    }

    void Update()
    {
        
    }

    public void Expand_Inventory()
    {
        int index = (int)current_Type;
        Base_Manager.inventory_Mng.Inventory_Size[index]++;
        Set_Inventory_Slot();
    }

    private void Set_Inventory_Slot() 
    {
        if (garvage_Slot.Count >=0)
        {
            for (int i = 0; i < garvage_Slot.Count; i++) 
            {
                Destroy(garvage_Slot[i]);
            }
            garvage_Slot.Clear();
        }

        List<Item_Holder> items_To_Display = new List<Item_Holder>();
        foreach (var item in Base_Manager.data_Mng.p_Item_Holder)
        {
            if (item.Value.holder.Quantity > 0 && item.Value.item_Data.item_Type == current_Type)
            {
                items_To_Display.Add(item.Value);
            }
        }

        int size = Get_Size(); 
        int index = 0;

        for (int i = 0; i < size; i++) 
        {
            Inventory_Item_Slot slot = Instantiate(slot_Prefab, content);
            garvage_Slot.Add(slot.gameObject);
            slot.gameObject.SetActive(true);

            if (index < items_To_Display.Count)
            {
                slot.Init(items_To_Display[index]);
                index++;
            }
            else
            {
                slot.Init(null);
            }
        }
        Button button = Instantiate(expand_Slot_Button, content);
        button.onClick.RemoveAllListeners();    
        button.onClick.AddListener(Expand_Inventory);
        button.gameObject.SetActive(true);
        garvage_Slot.Add(button.gameObject);
    }

    private int Get_Size() 
    {
        int size = 11;
        int index = (int)current_Type;
        int current_Size = Base_Manager.inventory_Mng.Inventory_Size[index] + 1;

        return size * current_Size;
    }

    private void Set_Item_Button_Type(Item_Type type) 
    {
        current_Type = type;
        Set_Inventory_Slot();

        if (select_Coroutine != null) 
        {
            StopCoroutine(select_Coroutine);
            select_Coroutine = null;
        }

        select_Coroutine = StartCoroutine(Select_Button());
    }

    public override void Close_UI()
    {   
        Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Clear();
        base.Close_UI();
    }


    private IEnumerator Select_Button()
    {
        float duration = 0.2f;
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
        Vector2 endPos = origin_Pos[Index] + new Vector2(7f, 0);

        Vector3 startScale = origin_Scale[Index];
        Vector3 endScale = origin_Scale[Index] * 1.1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            rect.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        rect.anchoredPosition = endPos;
        rect.localScale = endScale;
    }
}
