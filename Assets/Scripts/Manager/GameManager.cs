using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [Header("배달 장소")]
    public GameObject[] deliveryPoints;
    private int activeLocationsCount = 0;

    [Header("박스")]
    public Transform[] boxSpawnPoints;
    public GameObject boxPrefab;
    public List<GameObject> boxList = new List<GameObject>();
    private float boxSpawnTimer = 0f;

    [Header("자동차")]
    public Transform carSpawnPoint;
    public GameObject CarPrefab;
    public float carSpawnInterval = 2f;
    private float carSpawnTimer = 0f;

    [Header("폭탄")]
    public Vector2 explosionPoint;
    public GameObject dynamitePrefab;
    public float explosionSpawnInterval = 2f;
    private float dynamiteSpawnTimer = 0f;
    private GameObject target;

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

        target = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
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
        if(dynamiteSpawnTimer >= explosionSpawnInterval)
        {
            Bomb();
            dynamiteSpawnTimer = 0f;
        }
    }
    public void SpawnBox()
    {
        if (boxList.Count < maxBox)
        {
            for (int i = 0; i < 100; i++)
            {
                int index = Random.Range(0, boxSpawnPoints.Length);
                if (boxSpawnPoints[index].childCount == 0)
                {
                    GameObject box = Instantiate(boxPrefab, boxSpawnPoints[index].position,Quaternion.identity);
                    box.transform.SetParent(boxSpawnPoints[index], true);
                    //box.transform.localPosition = Vector2.zero;
                   boxList.Add(box);
                    break;
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
        Instantiate(CarPrefab, carSpawnPoint.position,Quaternion.identity);
    }

    public void Bomb()
    {
        explosionPoint = new Vector2(target.transform.position.x, target.transform.position.y -1);

        Instantiate(dynamitePrefab, explosionPoint, Quaternion.identity);
    }
}
