using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text BoxText;

    public GameObject Panel;

    public TMP_Text scoreText2;
    public TMP_Text BoxText2;
    public TMP_Text totalScoreText;

    public Button[] Btn;

    private int Score;
    private int Box;
    private float total;
    float curNum = 0f;

    bool isTotalCalculate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTotalCalculate && Input.anyKey && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            scoreText2.rectTransform.localScale = new Vector3(1f, 1f,1f);
            BoxText2.rectTransform.localScale = new Vector3(1f,1f,1f);
            curNum = total;
            totalScoreText.text = curNum.ToString();
            SetBtn();
        }
        
    }

    public void UpdateScore(int score)
    {
        Score = score;
        scoreText.text = "Score: " + score;
    }

    public void UpdateBox(int box)
    {
 
        Box = box;
        
        BoxText.text = "Box: " + box;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        isTotalCalculate = true;
        yield return new WaitForSeconds(0.1f); 
        scoreText2.text = scoreText.text;
        BoxText2.text = BoxText.text;
        if (Box == 0)
        {
            Box = 1;
        }
        total = Score * Box;
        scoreText2.rectTransform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.4f);
        BoxText2.rectTransform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.4f);
        

        while(curNum < total)
        {
            curNum++;
            yield return new WaitForSeconds(0.05f);
            totalScoreText.text = curNum.ToString();
        }

        SetBtn();
    }

    public void SetBtn()
    {
        Btn[0].gameObject.SetActive(true);
        Btn[1].gameObject.SetActive(true);
    }

    public void RetryBtn()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
