using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Inventory : UI_Base
{
    private List<GameObject> garvage_Slot = new List<GameObject>();

    [SerializeField]
    private Inventory_Item_Slot slot_Prefab;
    [SerializeField]
    private Button expand_Slot_Button;
    [SerializeField]
    private Transform content;

    private Item_Type current_Type = Item_Type.Weapon;
    [SerializeField]
    private Button[] item_Type_ButtonS = new Button[3];


    void Start()
    {
        Set_Inventory_Slot();

        item_Type_ButtonS[0].onClick.RemoveAllListeners();

        for (int i = 0; i < item_Type_ButtonS.Length; i++)
        {
            int index = i; 
            item_Type_ButtonS[i].onClick.RemoveAllListeners();
            item_Type_ButtonS[i].onClick.AddListener(() => Set_Item_Button_Type((Item_Type)index));
        }
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
    }

    public override void Close_UI()
    {   
        Base_Manager.inventory_Mng.Acrtion_Panal_Holder.Clear();
        base.Close_UI();
    }

}
