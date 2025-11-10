using UnityEngine;

public class Enemies_Visual_Animation : MonoBehaviour {

    //CONST PARAMS
    const string isWalking = "IsWalking";
    const string isHurt = "IsHurt";
    const string isDead = "IsDead";


    public static Enemies_Visual_Animation Instance { get; private set; }

    [SerializeField] Animator enemyAnimator;

    private void Awake() {
        Instance = this;
    }

    public void HandleHurtAnimation() {
        enemyAnimator.SetTrigger(isHurt);
    }

    public void HandleDeathAnimation() {
        enemyAnimator.SetTrigger(isDead);
    }


}
