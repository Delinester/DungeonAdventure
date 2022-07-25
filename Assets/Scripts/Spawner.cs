using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    private bool isReady = true;
    private int frequency = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady == true)
        {
            Spawn();
            StartCoroutine(WaitToSpawn());
        }         
    }

    private void Spawn()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], transform.position, transform.rotation);        
        isReady = false;
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(frequency);
        isReady = true;
    }
}
