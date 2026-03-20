using System.Collections;
using Unity.VisualScripting;
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
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            target = null;
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
