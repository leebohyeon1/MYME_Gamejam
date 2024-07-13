using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //=========================================================

    [Header("점수")]
    public int score = 0;
    public float scoreInterval = 0.1f;
    private float timer = 0f;
    private float gameTimer = 0f;
    [Space(10f)]
    public int maxBox = 3;
    public int boxCount = 0;
    [Space(10f)]
    public bool isGameOver;
    public float BestScore = 0;
    public string BestPlayer;

    [Header("배달 장소")]
    public GameObject[] deliveryPoints;
    private int activeLocationsCount = 0;

    public GameObject badBoxTrashCan;
    public GameObject ST_deliveryPoint;

    [Header("박스")]
    public GameObject[] boxPrefab;
    public int boxStack = 0;
    public List<GameObject> boxList = new List<GameObject>();
    private float boxSpawnTimer = 0f;
    private float badBoxTimer = 0f;
    private float goodBoxTimer = 0f;

    public GameObject[] ST_BoxSpawnPoints;

    [Header("자동차")]
    public Transform[] carSpawnPoint;
    public GameObject[] CarPrefab;
    public float carSpawnInterval = 2f;
    private float carSpawnTimer = 0f;

    [Header("폭탄")]
    public Vector2 explosionPoint;
    public GameObject dynamitePrefab;
    public float explosionSpawnInterval = 2f;
    private float dynamiteSpawnTimer = 0f;
    private GameObject target;

    [Header("좀비")]
    public GameObject[] zombieSpawnPoints;
    public GameObject[] Zombie;
    public List<GameObject> ZombieList = new List<GameObject>();
    public Camera mainCamera;
    public float ZombieSpawnInterval = 5f;
    private float ZombieSpawnTimer = 0f;

    public GameObject background;

    public float spawnDistance = 10.0f;

    public bool isCount;
    public bool isLoaging;

    void Start()
    {
        InitializeSingleton();
        LoadPlayerData();

        target = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        if (!isGameOver && SceneManager.GetActiveScene().buildIndex != 0 && !isCount)
        {
            UpdateTimers();
        }
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #region RandomMode

    private void LoadPlayerData()
    {
        BestPlayer = PlayerPrefs.GetString("BestPlayer", null);
        BestScore = PlayerPrefs.GetFloat("BestScore", 0f);
    }

    private void UpdateTimers()
    {
        gameTimer += Time.deltaTime;
        if (gameTimer >= 60)
        {
            score += score / 100 * 15;
            gameTimer = 0;
        }

        timer += Time.deltaTime;
        if (timer >= scoreInterval)
        {
            score++;
            UIManager.Instance.UpdateScoreText(score);
            timer = 0f;
        }

        boxSpawnTimer += Time.deltaTime;
        if (boxSpawnTimer >= 1f)
        {
            SpawnBox();
            boxSpawnTimer = 0f;
        }
        badBoxTimer += Time.deltaTime;
        if (badBoxTimer >= 24f)
        {
            SpawnBadBox();
        }
        goodBoxTimer += Time.deltaTime;
        if (goodBoxTimer >= 35f)
        {
            SpawnGoodBox();
        }
        carSpawnTimer += Time.deltaTime;
        if (carSpawnTimer >= carSpawnInterval)
        {
            SpawnCar();
            carSpawnTimer = 0f;
        }

        dynamiteSpawnTimer += Time.deltaTime;
        if (dynamiteSpawnTimer >= explosionSpawnInterval)
        {
            SpawnDynamite();
            dynamiteSpawnTimer = 0f;
        }

        ZombieSpawnTimer += Time.deltaTime;
        if (ZombieSpawnTimer >= ZombieSpawnInterval)
        {
            SpawnZombie();
            ZombieSpawnTimer = 0f;
        }
    }

    public void SpawnBox()
    {
        if (boxList.Count < maxBox)
        {
            int ranPoints = 0;
            int ranNum = Random.Range(0, 3);
            for (int i = 0; i < 100; i++)
            {
                ranPoints = Random.Range(0, ST_BoxSpawnPoints.Length);
                if (ST_BoxSpawnPoints[ranPoints].transform.childCount == 0)
                {
                    break;
                }
            }
            GameObject box = Instantiate(boxPrefab[ranNum], ST_BoxSpawnPoints[ranPoints].transform.position, Quaternion.identity);
            box.transform.SetParent(ST_BoxSpawnPoints[ranPoints].transform);
            boxList.Add(box);
        }
    }

    public void SpawnBadBox()
    {
        if (boxList.Count < maxBox)
        {
            badBoxTimer = 0f;
            int ranPoints = 0;
            for (int i = 0; i < 100; i++)
            {
                ranPoints = Random.Range(0, ST_BoxSpawnPoints.Length);
                if (ST_BoxSpawnPoints[ranPoints].transform.childCount == 0)
                {
                    break;
                }
            }
            GameObject box = Instantiate(boxPrefab[4], ST_BoxSpawnPoints[ranPoints].transform.position, Quaternion.identity);
            box.transform.SetParent(ST_BoxSpawnPoints[ranPoints].transform);
            boxList.Add(box);
        }
    }

    public void SpawnGoodBox()
    {
        if (boxList.Count < maxBox)
        {
            goodBoxTimer = 0f;
            int ranPoints = 0;
            for (int i = 0; i < 100; i++)
            {
                ranPoints = Random.Range(0, ST_BoxSpawnPoints.Length);
                if (ST_BoxSpawnPoints[ranPoints].transform.childCount == 0)
                {
                    break;
                }
            }
            GameObject box = Instantiate(boxPrefab[5], ST_BoxSpawnPoints[ranPoints].transform.position, Quaternion.identity);
            box.transform.SetParent(ST_BoxSpawnPoints[ranPoints].transform);
            boxList.Add(box);
        }
    }

    public void RemoveBoxList(GameObject box)
    {
        boxList.Remove(box);
    }

    public void ActivateLocation()
    {
        if (activeLocationsCount < maxBox)
        {
            for (int i = 0; i < 100; i++)
            {
                int index = Random.Range(0, deliveryPoints.Length);
                if (!deliveryPoints[index].activeSelf)
                {
                    deliveryPoints[index].SetActive(true);
                    activeLocationsCount++;
                    break;
                }
            }
        }
    }

    public void DeactivateLocation(GameObject location)
    {
        location.SetActive(false);
        activeLocationsCount--;
        boxCount++;
        UIManager.Instance.UpdateBoxText(boxCount);
    }

    public void SpawnCar()
    {
        foreach (var spawnPoint in carSpawnPoint)
        {
            Instantiate(CarPrefab[Random.Range(0, CarPrefab.Length)], spawnPoint.position, Quaternion.identity);
        }
    }

    public void SpawnDynamite()
    {
        explosionPoint = new Vector2(target.transform.position.x, target.transform.position.y - 1);
        Instantiate(dynamitePrefab, explosionPoint, Quaternion.identity);
    }

    void SpawnZombie()
    {
        if (ZombieList.Count >= 20) return;

        int num = Random.Range(0, Zombie.Length);
        int num1 = Random.Range(0, zombieSpawnPoints.Length);
        Vector3 spawnPosition = zombieSpawnPoints[num1].transform.position;
        GameObject zombie = Instantiate(Zombie[num], spawnPosition, Quaternion.identity);
        ZombieList.Add(zombie);
        IncreaseZombieSpeed();
    }

    private void IncreaseZombieSpeed()
    {
        foreach (GameObject zombie in ZombieList)
        {
            NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed += 0.5f;
            }
        }
    }

    public void ScoreSet(float score, string name)
    {
        BestScore = score;
        BestPlayer = name;
        PlayerPrefs.SetFloat("BestScore", score);
        PlayerPrefs.SetString("BestPlayer", name);
        PlayerPrefs.Save();
    }
    #endregion
}
