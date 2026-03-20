using System;
using System.Collections;
using UnityEngine;

public class Totem_Base : MonoBehaviour
{
    [SerializeField]
    private string totem_Name;
    [SerializeField]
    private float totem_Duration;
    [SerializeField]
    private Totem_Type totem_Type;
    [SerializeField]
    private GameObject start_Effect;
    [SerializeField]
    private GameObject totem_Effect;
    [SerializeField]
    private GameObject check_Player_Effect;

    public bool is_On = false;

    protected virtual void Start_Effect() 
    {
        is_On = true;   
        float value = start_Effect.GetComponent<ParticleSystem>().duration;
        if (coroutine != null) 
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(Start_Effect_Coroutine(value));
    }

    private Coroutine coroutine;
    IEnumerator Start_Effect_Coroutine(float delay) 
    {
        start_Effect.SetActive(true);
        yield return new WaitForSeconds(delay);
        totem_Effect.SetActive(true);

        yield return new WaitForSeconds(totem_Duration);
        On_Disable_Effect();
    }

    private void On_Disable_Effect() 
    {
        is_On = false;
        start_Effect.SetActive(false);
        totem_Effect.SetActive(false);
        check_Player_Effect.SetActive(false);
    }

    public void On_Check_Player_Effect(bool isCan)
    {
        if(check_Player_Effect == null || is_On) return;
        check_Player_Effect.SetActive(isCan);
    }
}
