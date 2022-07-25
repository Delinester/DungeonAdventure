using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] possibleWalls;
    public GameObject[] possibleDoors;
    public GameObject[] intersectionObjects;

    public GameObject[] objectsWithSpawnPossibility;

    [SerializeField]
    private GameObject[] enemies;
    private NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }
    private void Start()
    {
        int amountOfEnemies = Random.Range(1, 4);
        Vector3 spawnPoint = transform.position;
        for (int i = 0; i < amountOfEnemies; i++)
        {
            int index = Random.Range(0, enemies.Length);
            Enemy enemy = Instantiate(enemies[index], spawnPoint, enemies[index].transform.rotation).GetComponent<Enemy>();
            enemy.transform.SetParent(gameObject.transform);
            enemy.SetSurface(surface);
        }        
    }
    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            possibleWalls[i].SetActive(!status[i]);
            possibleDoors[i].SetActive(status[i]);
        }
    }
    public void DeactivateWall(int side)
    {        
        possibleWalls[side].SetActive(false);
        possibleDoors[side].SetActive(false);
        intersectionObjects[side].SetActive(false);
    }
}
