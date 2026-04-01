using UnityEngine;
using UnityEngine.SceneManagement;

public class Move_Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private string shop_Scene_Name = "Shop_Scene";

    public void Interact(Player_Interaction player)
    {
        if (string.IsNullOrEmpty(shop_Scene_Name))
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다!");
            return;
        }
        Scene_Data.spawn_Point_Name = "Shop_Spawn_Point";
        SceneManager.LoadScene(shop_Scene_Name);
    }
}