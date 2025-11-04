using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashDistance = 100f;

    private bool canMove = true;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnDashAction += GameInput_OnDashAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender,System.EventArgs e) {
        Debug.Log("ATTACKKKKK");
    }

    private void GameInput_OnDashAction(object sender,System.EventArgs e) {
        HandleDash();
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,inputVector.y,0f);

        float moveDistance = moveSpeed * Time.deltaTime;
        if(canMove) {
            transform.position += moveDir * moveDistance;
        }
    }
    private void HandleDash() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,inputVector.y,0f);

        if(canMove) {
            transform.position += moveDir * (dashDistance * Time.deltaTime);
        }
    }
}
