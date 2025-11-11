using UnityEngine;
using System.Collections;

public class Enemies_Visual_Animation : MonoBehaviour {
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int HurtHash = Animator.StringToHash("IsHurt");
    private static readonly int DeadBool = Animator.StringToHash("IsDead");

    [SerializeField] private Animator enemyAnimator;

    public void ResetAll() {
        if(!enemyAnimator) return;
        enemyAnimator.ResetTrigger(HurtHash);
        enemyAnimator.SetBool(DeadBool,false);
        enemyAnimator.SetBool(IsWalkingHash,false);
        // Optionally force Idle:
        // enemyAnimator.Play("Idle", 0, 0f);
    }

    public void SetWalking(bool walking) {
        if(!enemyAnimator) return;
        enemyAnimator.SetBool(IsWalkingHash,walking);
    }

    public void PlayHurt() {
        if(!enemyAnimator) return;
        enemyAnimator.ResetTrigger(HurtHash);
        enemyAnimator.SetBool(DeadBool,false);
        enemyAnimator.SetTrigger(HurtHash);
        // Or: enemyAnimator.CrossFadeInFixedTime("Hurt", 0.05f);
    }

    public void PlayDeath() {
        if(!enemyAnimator) return;
        enemyAnimator.ResetTrigger(HurtHash);
        enemyAnimator.SetBool(DeadBool,true);
        // Or: enemyAnimator.CrossFadeInFixedTime("Death", 0.05f);
    }

    public float GetClipLength(string stateName) {
        if(!enemyAnimator || enemyAnimator.runtimeAnimatorController == null) return 0f;
        foreach(var clip in enemyAnimator.runtimeAnimatorController.animationClips)
            if(clip && clip.name == stateName) return clip.length;
        return 0f;
    }

    // Wait until layer 0 is in `stateName` and almost finished
    public IEnumerator WaitForStateEnd(string stateName,float normalizedDone = 0.98f,float enterTimeout = 0.25f) {
        if(!enemyAnimator) yield break;

        // Wait until we enter the state (with small timeout in case of 0-length)
        float t = 0f;
        while(t < enterTimeout) {
            var info = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName(stateName)) break;
            t += Time.deltaTime;
            yield return null;
        }

        // Now wait for it to finish
        while(true) {
            var info = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName(stateName) && info.normalizedTime >= normalizedDone) break;
            yield return null;
        }
    }
}
