using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("Spawn Box")]
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask playerMask;

    [Header("Dog")]
    [SerializeField] private GameObject dog;
    [SerializeField] private Collider2D dogCol;
    private Vector2 dogOffset;
    private Vector2 dogSize;
    [SerializeField] private Vector2[] dogPositions;

    [Header("Robot")]
    [SerializeField] private GameObject robot;
    [SerializeField] private Collider2D robotCol;
    private Vector2 robotOffset;
    private Vector2 robotSize;
    [SerializeField] private Vector2[] robotPositions;

    private int maxSpawnCount;
    [HideInInspector] public int currentCount;
    private EnemyInfo[] enemies;

    private void Start()
    {
        maxSpawnCount = dogPositions.Length + robotPositions.Length;
        dogOffset = dogCol.offset;
        dogSize = dogCol.bounds.size;
        robotOffset = robotCol.offset;
        robotSize = robotCol.bounds.size;
        dog.SetActive(false);
        robot.SetActive(false);
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(transform.position, size, 0, playerMask) && currentCount < maxSpawnCount)
        {
            enemies = new EnemyInfo[FindObjectsOfType<EnemyInfo>().Length];
            enemies = FindObjectsOfType<EnemyInfo>();
            foreach (EnemyInfo enemy in enemies)
            {
                if(enemy.gameObject != transform.GetChild(0).gameObject && enemy.gameObject != transform.GetChild(1).gameObject)
                    Destroy(enemy.gameObject);
            }
            if (dogPositions.Length > 0)
            {
                for (int i = 0; i < dogPositions.Length; i++)
                {
                    GameObject temp = Instantiate(dog, dogPositions[i], Quaternion.identity);
                    temp.SetActive(true);
                    currentCount += 1;
                }
            }
            if (robotPositions.Length > 0)
            {
                for (int i = 0; i < robotPositions.Length; i++)
                {
                    GameObject temp = Instantiate(robot, robotPositions[i], Quaternion.identity);
                    temp.SetActive(true);
                    currentCount += 1;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, size);
        
        if (dogPositions.Length > 0)
        {
            for (int i = 0; i < dogPositions.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(dogPositions[i] + dogOffset, dogSize);
            }
        }
        if (robotPositions.Length > 0)
        {
            for (int i = 0; i < robotPositions.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(robotPositions[i] + robotOffset, robotSize);
            }
        }
        
    }

}
