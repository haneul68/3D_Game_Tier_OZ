using UnityEngine;

public class Totem : Totem_Base, IInteractable
{
    public void Interact(Player_Interaction player)
    {
        if (is_On) return;

        Start_Effect();
        Debug.Log("Èú ½ÃÀÛ");
    }
}
