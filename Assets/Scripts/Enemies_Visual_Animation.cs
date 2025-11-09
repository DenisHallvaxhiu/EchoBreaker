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

    public void HandleWalkAnimation(bool condition) {
        if(condition) {
            enemyAnimator.SetBool(isWalking,true);
        }
        else {
            enemyAnimator.SetBool(isWalking,false);
        }
    }
}
