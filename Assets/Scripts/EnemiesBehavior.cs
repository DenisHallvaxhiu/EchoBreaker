using UnityEngine;

public class EnemiesBehavior : MonoBehaviour {

    void Update()   // use Update with deltaTime when not using physics
    {
        float moveSpeed = SkeletonSoldier.Instance.GetMoveSpeed();
        Vector3 target = Player.Instance.transform.position;
        Vector3 dir = (target - transform.position).normalized; // direction *toward* player
        transform.position += dir * moveSpeed * Time.deltaTime; // move by speed
    }
}
