using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public TMP_Text Press;
    public TMP_Text LoadingText;
    bool isLoad;

    float timer = 0f;
    float timer1 = 0f;

    public float A = 0f;

    private void Awake()
    {
        isLoad = false;


       
    }
    void Start()
    {

        //StartCoroutine(Load());
        Press.color = new Color(1, 1, 1, 0);

    }

    void Update()
    {
        if(!isLoad)
        {
            timer += Time.deltaTime;
            timer1 += Time.deltaTime;
            if (timer > 2f)
            {
                LoadingText.gameObject.SetActive(false);
                if (timer1 > 0.2f)
                {
                    A += 0.1f;
                    Press.color = new Color(1, 1, 1, A);
                    if (A >= 1)
                    {
                        Load();
                    }
                    timer1 = 0f;
                }
                
            }
        }
       if(isLoad && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("Count", 1);
        }
    }

    public void Load()
    {
        isLoad = true;
        Press.gameObject.SetActive(true);
        //Press.color = new Color(1, 1, 1, 1);
    }
}
