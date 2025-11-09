using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashDistance = 100f;

    [Header("Attacks")]
    [SerializeField] private float attackRange = 50f;
    [SerializeField] private LayerMask enemiesLayerMask;


    private bool canMove = true;
    private Vector2 lastMoveDir;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnDashAction += GameInput_OnDashAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender,System.EventArgs e) {
        PlayerAnimation.Instance.HandleAttackAnimation();
        Attack();
    }

    private void GameInput_OnDashAction(object sender,System.EventArgs e) {
        HandleDash();
    }

    private void Update() {
        Vector3 moveDir = GameInput.Instance.GetMoveDir();
        HandleMovement(moveDir);
        PlayerAnimation.Instance.HandleRunAnimation(moveDir);



    }

    private void HandleMovement(Vector3 moveDir) {


        float moveDistance = moveSpeed * Time.deltaTime;
        if(canMove) {
            transform.position += moveDir * moveDistance;
            if(moveDir.x < 0f) {
                transform.localScale = new Vector3(-1,1,1);
            }
            else if(moveDir.x > 0f) {
                transform.localScale = new Vector3(1,1,1);
            }
        }



        if(moveDir != Vector3.zero) {
            lastMoveDir = moveDir;
        }
    }
    private void HandleDash() {
        Vector3 moveDir = GameInput.Instance.GetMoveDir();

        if(canMove) {
            transform.position += moveDir * (dashDistance * Time.deltaTime);
        }
    }

    private void Attack() {
        //Testing Attack raycast
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin,lastMoveDir,attackRange,enemiesLayerMask);

        if(hit.collider == null) return;

        if(hit.collider.TryGetComponent<IDamageable>(out var damageable)) {
            damageable.OnDamage(1);
            return;
        }
    }


}
