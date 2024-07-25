using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;

    public GameObject bulletPrefab;
    public int maxBulletPool = 10;
    public List<GameObject> bulletPoolList;

    public GameObject[] enemyPrefabs; // Changed to array
    private int maxEnemyPool = 10;
    public List<GameObject> enemyPoolList;
    public List<Transform> SpawnPointList;

    void Awake()
    {
        if (poolingManager == null)
            poolingManager = GetComponent<ObjectPoolingManager>();

        else if (poolingManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        bulletPrefab = Resources.Load("Bullet") as GameObject;
        enemyPrefabs = new GameObject[3]; // Initialize with the number of prefabs
        enemyPrefabs[0] = Resources.Load<GameObject>("Monster"); // Assuming Enemy1 and Enemy2 are prefab names
        enemyPrefabs[1] = Resources.Load<GameObject>("Skeleton");
        enemyPrefabs[2] = Resources.Load<GameObject>("Zombie");

        CreateBullet();
        CreateEnemy();
    }

    private void Start()
    {
        var SpawnPoint = GameObject.Find("SpawnPoints");

        if (SpawnPoint != null)
            SpawnPoint.GetComponentsInChildren<Transform>(SpawnPointList);

        SpawnPointList.RemoveAt(0);
    }

    void CreateBullet()
    {
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");

        for (int i = 0; i < 10; i++)
        {
            var bullets = Instantiate(bulletPrefab, playerBulletGroup.transform);
            bullets.name = $"{(i + 1).ToString()}발";
            bullets.SetActive(false);

            bulletPoolList.Add(bullets);
        }
    }

    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {
            if (bulletPoolList[i] != null && !bulletPoolList[i].activeSelf)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }

    void CreateEnemy()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < maxEnemyPool; i++)
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], EnemyGroup.transform);
            enemy.name = $"enemy_{i + 1}";
            enemy.SetActive(true);
            //true로 갈길까
            enemyPoolList.Add(enemy);
        }
    }

    public GameObject GetEnemyPool()
    {
        for (int i = 0; i < enemyPoolList.Count; i++)
        {
            if (!enemyPoolList[i].activeSelf)
            {
                return enemyPoolList[i];
            }
        }
        return null;
    }

}
