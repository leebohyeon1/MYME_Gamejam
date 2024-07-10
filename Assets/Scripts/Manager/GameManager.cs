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
    private float spawnTimer;


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
       
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 1f)
        {
            SpawnBox();
            spawnTimer = 0f;
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
                    GameObject box = Instantiate(boxPrefab, boxSpawnPoints[index]);
                    box.transform.SetParent(boxSpawnPoints[index], true);
                    box.transform.localPosition = Vector2.zero;
                   boxList.Add(box);
                    break;
                }
            }
        }
    }

    public void RemoveList(GameObject box)
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
}
