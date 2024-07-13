using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class TitleUI : MonoBehaviour
{
    public GameObject title;

    public TMP_Text nameText;
    public TMP_Text score;

    public GameObject Loading;
    public GameObject[] Btn;

    int BtnIndex = 0;
    //==========================================================

    void Start()
    {
        title.transform.DOPunchScale(new Vector3(0.5f,0.5f,0.5f),2f,2,0).SetEase(Ease.InCubic);
        Btn[0].transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
        if (PlayerPrefs.HasKey("BestPlayer"))
        {
            nameText.text = GameManager.Instance.BestPlayer;
            score.text = GameManager.Instance.BestScore.ToString();
        }
       
    }


    void Update()
    {
        if (UIManager.Instance.optionUI.gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (BtnIndex < Btn.Length -1)
            {
                BtnIndex++;
            }
            else
            {
                BtnIndex = Btn.Length -1;
            }

            for(int i = 0; i < Btn.Length; i++)
            { 
                if(i != BtnIndex)
                Btn[i].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
            }
            Btn[BtnIndex].transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Btn);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(BtnIndex > 0)
            {
                BtnIndex--;
            }
            else
            {
                BtnIndex = 0;
            }
            for (int i = 0; i < Btn.Length; i++)
            {
                if (i != BtnIndex)
                    Btn[i].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
            }
            Btn[BtnIndex].transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Btn);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            switch(BtnIndex)
            {
                case 0:
                    StartBtn();
                    break;
                case 1:
                    OptionBtn();
                    break;
                case 2:
                    ExitBtn();
                    break;
            }
        }
        
    }
    //==========================================================

    #region Button
    public void StartBtn()
    {
        PlayerPrefs.SetInt("Count", 1);
        Loading.SetActive(true);
        GameManager.Instance.isLoaging = true;
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void OptionBtn()
    {
        UIManager.Instance.OptionUISet(true);
    }
    #endregion

}
