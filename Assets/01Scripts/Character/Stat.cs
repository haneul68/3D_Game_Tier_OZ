using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float Base_Value;
    private List<float> modifiers = new List<float>();

    public float Final_Value
    {
        get
        {
            float final = Base_Value;

            foreach (var mod in modifiers)
            {
                final += mod;
            }

            return final;
        }
    }

    public void Add_Modifier(float value)
    {
        modifiers.Add(value);
    }

    public void Remove_Modifier(float value)
    {
        modifiers.Remove(value);
    }
}
