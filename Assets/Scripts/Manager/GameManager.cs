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
    public int boxCount = 0;

    [Header("배달 장소")]
    public GameObject[] deliveryPoints;
    public int maxBox = 3;
    private int activeLocationsCount = 0; 


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
