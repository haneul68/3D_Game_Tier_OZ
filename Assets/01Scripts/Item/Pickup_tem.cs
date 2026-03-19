using UnityEngine;

public class Pickup_Item : Item_Base
{
    [SerializeField]
    private Orb_Type type;
    [SerializeField]
    private float value = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Check_Item_Type(player);
            Destroy(gameObject);
        }
    }
    private void Check_Item_Type(Player player) 
    {
        switch (type) 
        {
            case Orb_Type.Health:
                player.Heal(value);
                break;
        }
    }
}