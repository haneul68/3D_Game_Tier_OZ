using UnityEngine;

public class Player_Interaction : MonoBehaviour
{

    private IInteractable current_Interactable;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (current_Interactable != null)
            {
                current_Interactable.Interact(this);
            }
            else
            {
                return;
            }
        }
    }
}
