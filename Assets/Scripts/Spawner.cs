using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Character chara;       
    public GameObject enemy;                
    public Transform[] spawnPoints;
    private int time=5;
    private int waveCount = 1;

    private void Start()
    {
        InitSpawn(10);
    }

    private void Update()
    {
        if (chara.score >= 3 * waveCount)
        {
            Spawn();
            waveCount++;
        }

    }


    void Spawn()
    {
        // If the player has no health left...
        if (chara.currentHealth <= 0)
        {
            return;
        }
        StartCoroutine(chara.showText(waveCount));
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex;
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        for (int i = 0; i < chara.score * 3; i++)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }

    void InitSpawn(int enemies)
    {
        StartCoroutine(chara.showText(waveCount));


        int spawnPointIndex;
        for (int i = 0; i < enemies; i++)
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
        waveCount++;
        
    }

   /* IEnumerator showText(int waveCount)
    {
        waveText.gameObject.SetActive(true);
        waveText.text = "Wave " + waveCount;
        waveText.CrossFadeAlpha(0.0f, time, false);
        yield return new WaitForSeconds(time);
        waveText.CrossFadeAlpha(1.0f, time, false);
        waveText.gameObject.SetActive(false);
    }*/


}