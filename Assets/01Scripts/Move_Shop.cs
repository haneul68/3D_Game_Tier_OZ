using UnityEngine;
using UnityEngine.SceneManagement;

public class Move_Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private string shop_Scene_Name = "Shop_Scene";
    [SerializeField] private string Spwan_Point_Name = "";
    [SerializeField] bool is_RPG_Disabled_Scene = false;

    public bool Is_Interactable { get; set; }

    public void Interact(Player_Interaction player)
    {
        if (string.IsNullOrEmpty(shop_Scene_Name))
        {
            Debug.LogError("씬 이름이 설정x");
            return;
        }
        Scene_Data.spawn_Point_Name = Spwan_Point_Name;
        Scene_Data.is_RPG_Disabled_Scene = is_RPG_Disabled_Scene;

        if (is_RPG_Disabled_Scene)
        {
            Base_Manager.game_Mng.is_RPG_Disabled = true;

            if (Base_Manager.game_Mng.current_Mode == Camera_Mode.RPG)
            {
                Base_Manager.game_Mng.current_Mode = Camera_Mode.TPS;
                Base_Manager.game_Mng.ui_Selected_Mode = Camera_Mode.TPS;
            }
        }
        else
        {
            Base_Manager.game_Mng.is_RPG_Disabled = false;
        }

        SceneManager.LoadScene(shop_Scene_Name);
    }
}