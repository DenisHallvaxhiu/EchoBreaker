using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    [Header("Spawner Settings")]
    [SerializeField] float spawnRate = 100;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject player;


    [SerializeField] GameObject enemyPrefab;
    void Start() {
        StartCoroutine(SpawnTimer());
    }


    public void SpawnPrefabAtRuntime(GameObject objectToSpawn) {

        Vector2 spawnPosition = SpawnOnRectEdge(player.transform.position,4);

        Instantiate(objectToSpawn,spawnPosition,Quaternion.identity);
    }

    IEnumerator SpawnTimer() {

        while(true) {
            int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

            yield return new WaitForSeconds(SpawnRateCalculator(spawnRate));
            if(currentEnemies < maxEnemies) {
                SpawnPrefabAtRuntime(enemyPrefab);
            }
        }
    }


    Vector2 SpawnOnRectEdge(Vector2 center,float distanceFromCenter) {
        int edge = Random.Range(0,4);
        switch(edge) {
            case 0:
            return new Vector2(Random.Range(center.x - distanceFromCenter,center.x + distanceFromCenter),center.y + distanceFromCenter);
            case 1:
            return new Vector2(Random.Range(center.x - distanceFromCenter,center.x + distanceFromCenter),center.y - distanceFromCenter);
            case 2:
            return new Vector2(center.x - distanceFromCenter,Random.Range(center.y - distanceFromCenter,center.y + distanceFromCenter));
            default:
            return new Vector2(center.x + distanceFromCenter,Random.Range(center.y - distanceFromCenter,center.y + distanceFromCenter));
        }
    }

    float SpawnRateCalculator(float rate) {
        return 100 / rate;
    }
}

