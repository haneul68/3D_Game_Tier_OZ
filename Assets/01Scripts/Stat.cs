using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float BaseValue;
    private List<float> modifiers = new List<float>();

    public float Final_Value
    {
        get
        {
            float final = BaseValue;

            foreach (var mod in modifiers)
            {
                final += mod;
            }

            return final;
        }
    }

    public void AddModifier(float value)
    {
        modifiers.Add(value);
    }

    public void RemoveModifier(float value)
    {
        modifiers.Remove(value);
    }
}
