using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class TitleUI : MonoBehaviour
{
    public GameObject title;

    public TMP_Text nameText;
    public TMP_Text score;
    //==========================================================

    void Start()
    {
        title.transform.DOPunchScale(new Vector3(0.5f,0.5f,0.5f),2f,2,0).SetEase(Ease.InCubic);

        if(GameManager.Instance.BestPlayer != null)
        {
            nameText.text = GameManager.Instance.BestPlayer;
            score.text = GameManager.Instance.BestScore.ToString();
        }
       
    }


    void Update()
    {
        
    }
    //==========================================================

    #region Button
    public void StartBtn()
    {
        SceneManager.LoadScene(1);
        EventManager.Instance.PostNotification(EVENT_TYPE.SCENE_LOAD, this, 1);
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
