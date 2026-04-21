using System.Collections;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab;
    public Transform target;
    public Transform[] spawnPoints;
    public float spawnInterval = 2.0f;

    void Start()
    {
        // Spustíme nekonečnú slučku na vytváranie dronov
        StartCoroutine(SpawnDronesRoutine());
    }

    IEnumerator SpawnDronesRoutine()
    {
        while (true) // Bude bežať počas celej hry
        {
            SpawnSingleDrone();
            // Počkáme daný interval pred vytvorením ďalšieho
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSingleDrone()
    {
        if (dronePrefab == null || spawnPoints.Length == 0 || target == null)
        {
            Debug.LogWarning("Spawner nie je správne nastavený v Inšpektore!");
            return;
        }

        // Náhodne vyberieme jeden zo spawn pointov
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Vytvoríme nový dron z prefabu na pozícii spawn pointu
        GameObject newDrone = Instantiate(dronePrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        // Povieme novému dronu, kam má letieť
        newDrone.GetComponent<DroneController>().SetTarget(target);
    }
}