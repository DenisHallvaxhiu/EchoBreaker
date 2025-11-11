using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashDistance = 100f;

    [Header("Attacks")]
    [SerializeField] private float attackRange = 50f;
    [SerializeField] private float attackPerSecond = 1f;
    [SerializeField] private float attackHitDelay = 0.12f;   // when the blade actually “connects” in the clip
    [SerializeField] private float attackRecover = 0.08f;    // small recovery after hit
    [SerializeField] private LayerMask enemiesLayerMask;


    [Header("Debug / Gizmos")]
    [SerializeField] private bool showConeGizmo = true;
    [SerializeField] private float gizmoConeAngle = 90f;   // matches attackAngle
    [SerializeField] private int gizmoSegments = 16;       // arc smoothness

    private float nextAttackTime = 0f;
    private bool canMove = true;
    private Vector2 lastMoveDir;

    private void Awake() { Instance = this; }

    private void Start() {
        gameInput.OnDashAction += GameInput_OnDashAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void Update() {
        Vector3 moveDir = GameInput.Instance.GetMoveDir();
        HandleMovement(moveDir);
        PlayerAnimation.Instance.HandleRunAnimation(moveDir);


        if(showConeGizmo && Application.isPlaying) {
            // Draw the two edge rays (visible in Scene/Game if Gizmos toggled on)
            Vector3 facing = lastMoveDir.sqrMagnitude > 0.0001f ? (Vector3)lastMoveDir.normalized : transform.right;
            float half = gizmoConeAngle * 0.5f;
            Vector3 leftDir = Quaternion.Euler(0,0,-half) * facing;
            Vector3 rightDir = Quaternion.Euler(0,0,half) * facing;

            Debug.DrawLine(transform.position,transform.position + leftDir * attackRange,Color.red);
            Debug.DrawLine(transform.position,transform.position + rightDir * attackRange,Color.red);
        }
    }

    private void HandleMovement(Vector3 moveDir) {
        if(canMove) {
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            if(moveDir.x < 0f) transform.localScale = new Vector3(-1,1,1);
            else if(moveDir.x > 0f) transform.localScale = new Vector3(1,1,1);
        }
        if(moveDir != Vector3.zero) lastMoveDir = moveDir;
    }

    private void GameInput_OnDashAction(object s,System.EventArgs e) => HandleDash();

    private void HandleDash() {
        Vector3 moveDir = GameInput.Instance.GetMoveDir();
        if(canMove) transform.position += moveDir * (dashDistance * Time.deltaTime);
    }

    private void GameInput_OnAttackAction(object s,System.EventArgs e) => TryAttack();

    private void TryAttack() {
        if(Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + (1f / attackPerSecond);   // set cooldown now
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine() {
        canMove = false;
        PlayerAnimation.Instance.HandleAttackAnimation();       // starts swing

        // Hit happens when the weapon visually connects:
        yield return new WaitForSeconds(attackHitDelay);
        DoConeHit();

        // Small recovery so the end of the clip matches control return:
        yield return new WaitForSeconds(attackRecover);
        canMove = true;
    }

    private void DoConeHit() {
        // Variables
        float attackAngle = 90f;           // Cone width in degrees (90 = wide cleave)
        float attackRadius = attackRange;  // how far the attack reaches

        // Get all enemies in this circle area
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,attackRadius,enemiesLayerMask);

        foreach(Collider2D hit in hits) {
            Vector2 toEnemy = (hit.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(lastMoveDir,toEnemy);

            if(angle <= attackAngle / 2f)  // inside cone
            {
                if(hit.TryGetComponent<IDamageable>(out var damageable)) {
                    damageable.OnDamage(1);
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if(!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);

        Vector3 dir = lastMoveDir;
        float halfAngle = 90f / 2f;

        Quaternion leftRot = Quaternion.Euler(0,0,-halfAngle);
        Quaternion rightRot = Quaternion.Euler(0,0,+halfAngle);

        Gizmos.DrawLine(transform.position,transform.position + (leftRot * dir) * attackRange);
        Gizmos.DrawLine(transform.position,transform.position + (rightRot * dir) * attackRange);
    }


}
