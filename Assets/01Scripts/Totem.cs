using UnityEngine;

public class Totem : Totem_Base, IInteractable
{
    public bool Is_Interactable { get; set; }

    public void Interact(Player_Interaction player)
    {
        if (is_On) return;

        Start_Effect(player);
        Debug.Log("Èú ½ÃÀÛ");
    }
}
