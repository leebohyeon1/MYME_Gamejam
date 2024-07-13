using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    //===================================================================
    public int goal;
    public int curBox;

    public GameObject deliveryPoint;

    public GameObject[] boxSpawnPoints;
    public GameObject[] boxPrefab;
    public int maxBox;
    private List<GameObject> boxList = new List<GameObject>();
    private float boxSpawnTimer = 0f;

    [Header("ÆøÅº")]
    public bool isAirStrike = false;
    public Vector2 explosionPoint;
    public GameObject bombPrefab;
    public float explosionSpawnInterval = 2f;
    private float bombSpawnTimer = 0f;
    private GameObject target;
  



    void Start()
    {
          if(isAirStrike)
          {
            target = FindFirstObjectByType<PlayerController>().gameObject;
          }
    }

    void Update()
    {
        bombSpawnTimer += Time.deltaTime;
        if (bombSpawnTimer >= explosionSpawnInterval)
        {
            SpawnDynamite();
            bombSpawnTimer = 0f;
        }

        boxSpawnTimer += Time.deltaTime;
        if (boxSpawnTimer >= 1f)
        {
            SpawnBox();
            boxSpawnTimer = 0f;
        }
    }

    public void SpawnDynamite()
    {
        explosionPoint = new Vector2(target.transform.position.x, target.transform.position.y - 1);
        Instantiate(bombPrefab, explosionPoint, Quaternion.identity);
    }

    public void SpawnBox()
    {

        if (boxList.Count < maxBox)
        {
            int ranPoints = Random.Range(0, boxSpawnPoints.Length);
            int ranBox = Random.Range(0, 3);
            GameObject box = Instantiate(boxPrefab[ranBox], boxSpawnPoints[ranPoints].transform.position, Quaternion.identity);
            boxList.Add(box);
        }
    }

}