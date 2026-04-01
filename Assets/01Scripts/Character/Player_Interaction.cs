using NUnit.Framework.Interfaces;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    Character character;
    private IInteractable current_Interactable;

    void Start()
    {
        if(character == null)
            character = GetComponentInParent<Character>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (current_Interactable != null)
            {
                current_Interactable.Interact(this);
                current_Interactable = null;
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            current_Interactable = interactable;
            if (interactable is Totem_Base totem)
            {
                totem.On_Check_Player_Effect(true);
            }

            Debug.Log("상호작용 대상이 존재합니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();

        if (interactable != null && current_Interactable == interactable)
        {
            current_Interactable = null;
            if (interactable is Totem_Base totem)
            {
                totem.On_Check_Player_Effect(false);
            }
            Debug.Log("상호작용 대상에서 멀어졌습니다.");
        }
    }
}
