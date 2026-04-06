using System.Collections;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField]
    private Zone_Type zone_Type;
    [SerializeField]
    private int zone_Value;
    [SerializeField]
    private float delay;

    private IDamageable target;

    private Coroutine coroutine;

    private Camera_Mode previousMode;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable idamageable = other.GetComponent<IDamageable>();
        if (idamageable != null) 
        {
            target = idamageable;
            Start_Zone_Effect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamageable>() == target)
        {
            Stop_Zone_Effect();
        }
    }

    private void Start_Zone_Effect() 
    {
        if(coroutine != null) 
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        switch (zone_Type) 
        {
            case Zone_Type.Heal:
                coroutine = StartCoroutine(Start_Heal_Coroutine());
                break;
            case Zone_Type.FPS_Zone:
                previousMode = Base_Manager.game_Mng.current_Mode;
                Base_Manager.game_Mng.current_Mode = Camera_Mode.FPS;
                Base_Manager.game_Mng.is_Camera_Mode_Locked = true;
                break;
        }
    }

    private void Stop_Zone_Effect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        switch (zone_Type)
        {
            case Zone_Type.Heal:
                break;
            case Zone_Type.FPS_Zone:
                Base_Manager.game_Mng.is_Camera_Mode_Locked = false;
                Base_Manager.game_Mng.Restore_Mode_From_UI();
                break;
        }
    }

    IEnumerator Start_Heal_Coroutine() 
    {
        while (target != null) 
        {
            target.Heal(zone_Value);
            yield return new WaitForSeconds(delay);
        }
    }
}
