using System.Collections;
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
    public float scoreInterval = 1f;
    private float timer = 0f;
    [Space(10f)]
    public int maxBox = 3;
    public int boxCount = 0;
    [Space(10f)]
    public float totalScore;
    public bool isGameOver;
    public float BestScore = 0;
    public string BestPlayer;

    [Header("배달 장소")]
    public GameObject[] deliveryPoints;
    private int activeLocationsCount = 0;

    [Header("박스")]
    //public Transform[] boxSpawnPoints;
    public GameObject[] boxPrefab;
    public int boxPreCount = 0;
    public List<GameObject> boxList = new List<GameObject>();
    private float boxSpawnTimer = 0f;

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
    public GameObject[] Zombie;
    public List<GameObject> ZombieList = new List<GameObject>();
    public Camera mainCamera;
    public float ZombieSpawnInterval = 10f;
    private float ZombieSpawnTimer = 0f;

    public GameObject background;

    public float spawnDistance = 10.0f;

    public bool isCount;

    void Start()
    {

        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
           

        BestPlayer = PlayerPrefs.GetString("BestPlayer", null);
        BestScore = PlayerPrefs.GetFloat("BestScore", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver && SceneManager.GetActiveScene().buildIndex != 0 && !isCount)
        {
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

            carSpawnTimer += Time.deltaTime;
            if (carSpawnTimer >= carSpawnInterval)
            {
                SpawnCar();
                carSpawnTimer = 0f;
            }

            dynamiteSpawnTimer += Time.deltaTime;
            if (dynamiteSpawnTimer >= explosionSpawnInterval)
            {
                Bomb();
                dynamiteSpawnTimer = 0f;
            }

            ZombieSpawnTimer += Time.deltaTime;
            if (ZombieSpawnTimer >= ZombieSpawnInterval)
            {
                SpawnZombie();
                foreach(GameObject Zom in ZombieList)
                {
                    Zom.GetComponent<NavMeshAgent>().speed += 0.5f;
                }
                ZombieSpawnTimer = 0f;
            }
        }
       
    }
    public void SpawnBox()
    {
        if (boxList.Count < maxBox)
        {
            Vector3 spawnPosition;
            if (TryGetRandomNavMeshLocation(out spawnPosition))
            {
                GameObject box = Instantiate(boxPrefab[boxPreCount], spawnPosition, Quaternion.identity);
                boxPreCount++;
                boxList.Add(box);
                if(boxPreCount > maxBox)
                {
                    boxPreCount = 0;
                }

            }
        }
    }

    public void RemoveBoxList(GameObject box)
    {
        for(int i = 0; i < boxList.Count; i++)
        {
            if (boxList[i] == box)
            {
                boxList.RemoveAt(i);
            }
        }
        boxPreCount--;
    }

    public void ActivateLocation()
    {
        if (activeLocationsCount < maxBox)
        {
            for(int i = 0; i < 100; i++)
            {
                int index = Random.Range(0, deliveryPoints.Length);
                if(!deliveryPoints[index].activeSelf)
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
        for(int i = 0; i < carSpawnPoint.Length; i++)
        {
            Instantiate(CarPrefab[i], carSpawnPoint[i].position, Quaternion.identity);
        }
       
    }

    public void Bomb()
    {
        explosionPoint = new Vector2(target.transform.position.x, target.transform.position.y -1);

        Instantiate(dynamitePrefab, explosionPoint, Quaternion.identity);
    }

    void SpawnZombie()
    {
        if (ZombieList.Count == 15)
        {
            return;
        }
        int num = Random.Range(0, Zombie.Length);
        Vector3 spawnPosition = GetRandomPositionOutsideCamera();
        GameObject jom = Instantiate(Zombie[num], spawnPosition, Quaternion.identity);
        ZombieList.Add(jom);
    }

    Vector3 GetRandomPositionOutsideCamera()
    {
        Bounds bounds = background.GetComponent<SpriteRenderer>().bounds;
        Vector3 spawnPosition = Vector3.zero;
        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        // 카메라의 현재 위치
        Vector3 cameraPosition = mainCamera.transform.position;

        float spawnX, spawnY;

        // X 위치 선택
        if (Random.value < 0.5f)
        {
            // 왼쪽 또는 오른쪽
            spawnX = (Random.value < 0.5f) ? cameraPosition.x - horizontalExtent - spawnDistance : cameraPosition.x + horizontalExtent + spawnDistance;
            spawnX = Mathf.Clamp(spawnX, bounds.min.x, bounds.max.x);
            spawnY = Random.Range(bounds.min.y, bounds.max.y);
        }
        else
        {
            // 위쪽 또는 아래쪽
            spawnY = (Random.value < 0.5f) ? cameraPosition.y - verticalExtent - spawnDistance : cameraPosition.y + verticalExtent + spawnDistance;
            spawnY = Mathf.Clamp(spawnY, bounds.min.y, bounds.max.y);
            spawnX = Random.Range(bounds.min.x, bounds.max.x);
        }

        spawnPosition = new Vector3(spawnX, spawnY, 0);
        return spawnPosition;
    }

    bool TryGetRandomNavMeshLocation(out Vector3 resultPosition)
    {
        Bounds bounds = background.GetComponent<SpriteRenderer>().bounds;

        int attempts = 30;  // 시도 횟수
        while (attempts > 0)
        {
            attempts--;
            Vector3 randomPosition = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y), 0  // Y 축은 평면 게임의 경우 0 또는 특정 Y 축 값 설정
                
            );

            NavMeshHit hit;
            float maxDistance = 3.0f;  // NavMesh 검색 반경
            if (NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas))
            {
                resultPosition = hit.position;
                return true;
            }
        }

        resultPosition = Vector3.zero;
        return false;
    }

    public void ScoreSet(float score,string name) 
    {
        BestScore = score;
        BestPlayer = name;

        PlayerPrefs.Save();
    }
}
