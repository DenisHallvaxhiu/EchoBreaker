using UnityEngine;
using System.Collections;

public class SkeletonSoldier : MonoBehaviour, IDamageable {
    [Header("Skeleton Soldier Stats")]
    [SerializeField] private float maxHealth = 2f;
    [SerializeField] private float iFrameTime = 0.2f;
    [SerializeField] private float moveSpeed = 1f;

    [Header("References")]
    [SerializeField] private Enemies_Visual_Animation visuals; // child with Animator

    // (Optional) assign if you want to stop hitboxes/overlaps on death
    [SerializeField] private Collider[] colliders3D;
    [SerializeField] private Collider2D[] colliders2D;

    private float currentHealth;
    private bool invulnerable;
    private bool isDead;
    private bool isHurting;

    // Public API for other scripts
    public float MoveSpeed => moveSpeed;
    public bool IsDead => isDead;
    public bool CanMove => !isDead && !isHurting;

    private void Awake() {
        if(!visuals) visuals = GetComponentInChildren<Enemies_Visual_Animation>(true);
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        currentHealth = maxHealth;
        invulnerable = false;
        isDead = false;
        isHurting = false;
        visuals?.ResetAll();
    }

    public void OnDamage(int amount = 1) {
        if(invulnerable || isDead) return;

        currentHealth -= amount;

        if(currentHealth <= 0f) {
            StartCoroutine(DeathSequence());
        }
        else if(iFrameTime > 0f) {
            StartCoroutine(HurtSequence());
        }
    }

    private IEnumerator HurtSequence() {
        isHurting = true;
        invulnerable = true;

        visuals?.PlayHurt();                // quick, interruptible hit-react

        yield return new WaitForSeconds(iFrameTime);

        invulnerable = false;
        isHurting = false;
    }

    private IEnumerator DeathSequence() {
        isDead = true;

        // stop future interactions (optional but recommended)
        if(colliders3D != null) foreach(var c in colliders3D) if(c) c.enabled = false;
        if(colliders2D != null) foreach(var c in colliders2D) if(c) c.enabled = false;

        visuals?.PlayDeath();              // locks to Death state via IsDead bool inside visuals

        // Wait for the actual Death state to finish, then despawn
        if(visuals != null) {
            // requires the helper methods shown earlier in Enemies_Visual_Animation:
            // GetClipLength("Death") and WaitForStateEnd("Death", ...)
            yield return visuals.WaitForStateEnd("Death",0.98f,0.25f);
        }
        else {
            yield return new WaitForSeconds(0.6f); // fallback
        }

        Destroy(gameObject);
    }

    // Kept for compatibility if other scripts still call a method:
    public float GetMoveSpeed() => moveSpeed;

    // Optional external kill (e.g., from traps)
    public void Die() { if(!isDead) StartCoroutine(DeathSequence()); }
}
