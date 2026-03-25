using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Item_Atlas");
    public static Sprite Get_Atlas(string temp)
    {
        if (atlas == null) Debug.Log("¾øÀ½");
        return atlas.GetSprite(temp);
    }
}
