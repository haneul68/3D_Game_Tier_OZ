using UnityEngine;
using System.Collections;

public class Return_S : MonoBehaviour
{
    public string name;
    [SerializeField]
    private float delay;

    private Coroutine coroutine;

    public void Start_Return_Coroutine() 
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(ReturnAfterDelay());
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Base_Manager.pool_Mng.pool_Dictionary[name].Return(this.gameObject);
        Debug.Log("∏Æ≈œ");
    }
}