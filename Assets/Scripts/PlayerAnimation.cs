using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    const string IS_RUNNING = "isRunning";
    const string IS_ATTACKING = "isAttacking";

    public static PlayerAnimation Instance { get; private set; }
    [SerializeField] private Animator animator;

    private void Awake() {
        Instance = this;
    }

    public void HandleRunAnimation(Vector3 input) {
        if(input != Vector3.zero) {
            animator.SetBool(IS_RUNNING,true);
        }
        else {
            animator.SetBool(IS_RUNNING,false);
        }
    }

    public void HandleAttackAnimation() {
        animator.SetTrigger(IS_ATTACKING);
    }
}
