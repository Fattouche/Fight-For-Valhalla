using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//this is the spawner for all the enemies
public class Spawner : MonoBehaviour
{
    public Character character;       
    public GameObject enemy;                
    public Transform[] spawnPoints;
    private int time = 5;
    private int waveCount = 1;

    private void Start()
    {
        InitSpawn(10);
    }

    //spawn in waves
    private void Update()
    {
        if (character.score >= 3 * waveCount)
        {
            Spawn();
            waveCount++;
        }

    }

    //spwan depending on the characters score, ie higher score means more enemies
    void Spawn()
    {
        // If the player has no health left we stop the game
        if (character.currentHealth <= 0)
        {
            return;
        }
        StartCoroutine(character.showText(waveCount));
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex;
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        for (int i = 0; i < character.score * 3; i++)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }

    //this is just so that we can spawn a fixed number of enemies at the start
    void InitSpawn(int enemies)
    {
        StartCoroutine(character.showText(waveCount));


        int spawnPointIndex;
        for (int i = 0; i < enemies; i++)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
        waveCount++;
        
    }
}