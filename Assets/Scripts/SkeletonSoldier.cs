using UnityEngine;


public class SkeletonSoldier : MonoBehaviour, IDamageable {

    [Header("SkeletonSolder Stats")]
    [SerializeField] private float maxHealth = 2f;
    [SerializeField] private float iFrameTime = 0.2f;

    //Enemies_Visual_Animation animation;

    float currentHealth;
    bool invulnerable;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        currentHealth = maxHealth;
        invulnerable = false;
    }

    public void OnDamage(int amount = 1) {
        if(invulnerable || currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log($"[{name}] HP: {currentHealth}");


        if(currentHealth <= 0)
            Die();
        else {
            if(iFrameTime > 0) {
                StartCoroutine(IFrames());
            }
        }
    }

    private void Die() {
        Destroy(gameObject);
    }

    System.Collections.IEnumerator IFrames() {
        invulnerable = true;



        yield return new WaitForSeconds(iFrameTime);
        invulnerable = false;
    }
}
