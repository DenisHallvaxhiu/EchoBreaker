using UnityEngine;

[RequireComponent(typeof(SkeletonSoldier))]
public class EnemiesBehavior : MonoBehaviour {
    private SkeletonSoldier stats;
    private Transform player;

    void Awake() {
        stats = GetComponent<SkeletonSoldier>();
    }

    void Start() {
        if(Player.Instance) player = Player.Instance.transform;
    }

    void Update() {
        if(!player || !stats.CanMove) return;

        Vector3 toPlayer = player.position - transform.position;
        if(toPlayer.sqrMagnitude < 0.0001f) return;

        transform.position += toPlayer.normalized * stats.MoveSpeed * Time.deltaTime;
    }
}
