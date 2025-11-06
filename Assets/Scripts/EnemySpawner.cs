using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] float spawnRate = 1;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        StartCoroutine(SpawnTimer());
    }

    public void SpawnPrefabAtRuntime() {
        Vector2 center = player.transform.position;

        float randomX = Random.Range(center.x + 4,center.x - 4);
        float randomY = Random.Range(center.y + 4,center.y - 4);

        Vector2 spawnPosition = new Vector2(randomX,randomY);

        Instantiate(enemyPrefab,spawnPosition,Quaternion.identity);
    }

    IEnumerator SpawnTimer() {

        Debug.Log("Spawn start");
        yield return new WaitForSeconds(2f);
        Debug.Log("Spawned");
        SpawnPrefabAtRuntime();
    }
}
