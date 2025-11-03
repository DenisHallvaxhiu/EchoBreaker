using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float MoveSpeed = 7f;

    private bool canMove = true;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnDashAction += GameInput_OnDashAction;
    }

    private void GameInput_OnDashAction(object sender,System.EventArgs e) {

        Debug.Log("Dashed");

    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,inputVector.y,0f);

        float moveDistance = MoveSpeed * Time.deltaTime;
        if(canMove) {
            transform.position += moveDir * moveDistance;
        }
    }
}
