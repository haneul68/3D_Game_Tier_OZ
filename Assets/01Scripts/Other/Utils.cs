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
public static class AnimatorHash
{
    public static readonly int isJump = Animator.StringToHash("isJump");
    public static readonly int isAttack1 = Animator.StringToHash("isAttack_1");
    public static readonly int isAttack2 = Animator.StringToHash("isAttack_2");
    public static readonly int isAttack3 = Animator.StringToHash("isAttack_3");
    public static readonly int isDie = Animator.StringToHash("isDie");
    public static readonly int isHit = Animator.StringToHash("isHit");

    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isWalk_F = Animator.StringToHash("isWalk_F");
    public static readonly int isWalk_L = Animator.StringToHash("isWalk_L");
    public static readonly int isWalk_R = Animator.StringToHash("isWalk_R");
    public static readonly int isWalk_B = Animator.StringToHash("isWalk_B");
    public static readonly int isRun = Animator.StringToHash("isRun");
}