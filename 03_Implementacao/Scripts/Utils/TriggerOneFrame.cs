using System.Collections;
using UnityEngine;

public static class AnimatorExtension
{
    public static void SetTriggerOneFrame(this Animator anim, MonoBehaviour coroutineRunner, string trigger,
        float waitSeconds)
    {
        coroutineRunner.StartCoroutine(TriggerOneFrame(anim, trigger, waitSeconds));
    }

    private static IEnumerator TriggerOneFrame(Animator anim, string trigger, float waitSeconds)
    {
        anim.SetTrigger(trigger);
        if (waitSeconds == 0) yield return null;
        else yield return new WaitForSeconds(waitSeconds);
        anim.ResetTrigger(trigger);
    }
}