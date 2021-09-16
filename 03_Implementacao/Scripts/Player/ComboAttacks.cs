using UnityEngine;

public static class ComboAttacks
{
    public static int Combo(AnimatorStateInfo animation, int numOfLeftClicks, int numOfRightClicks)
    {
        if (animation.IsName("LightAttk 1") && numOfLeftClicks == 1 ||
            animation.IsName("LightAttk 2") && numOfLeftClicks == 2 ||
            animation.IsName("LightAttk 3") && numOfLeftClicks == 3 ||
            animation.IsName("StrongAttk 5") && numOfRightClicks == 1 ||
            animation.IsName("StrongAttk 6") && numOfRightClicks == 2 ||
            animation.IsName("StrongAttk 7") && numOfRightClicks == 3)
            return 0;

        // Light Combo
        // if first animation still playing and at least 2 clicks, next combo anim
        if (animation.IsName("LightAttk 1") && numOfLeftClicks >= 2)
            return 2;

        // if second animation still playing and at least 3 clicks, next combo anim
        if (animation.IsName("LightAttk 2") && numOfLeftClicks >= 3)
            return 3;

        // if third animation still playing and at least 3 clicks, next combo anim
        if (animation.IsName("LightAttk 3") && numOfLeftClicks >= 4)
            return 4;

        // fourth animation is last, return to idle
        if (animation.IsName("LightAttk 4"))
            return 0;
        
        // Strong Combo
        // if first animation still playing and at least 2 clicks, next combo anim
        if (animation.IsName("StrongAttk 5") && numOfRightClicks >= 2)
            return 6;

        // if second animation still playing and at least 3 clicks, next combo anim
        if (animation.IsName("StrongAttk 6") && numOfRightClicks >= 3)
            return 7;

        // if third animation still playing and at least 3 clicks, next combo anim
        if (animation.IsName("StrongAttk 7") && numOfRightClicks >= 4)
            return 8;

        // fourth animation is last, return to idle
        if (animation.IsName("StrongAttk 8"))
            return 0;

        return 0;
    }
}