using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IListener
{ 
    public static UIManager Instance { get; private set; }

    public GameUI gameUI;
    public TitleUI titleUI;
    public OptionUI optionUI;
    //==========================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SCENE_LOAD, this);

        if (titleUI == null) { titleUI = FindObjectOfType<TitleUI>(); }
        if(optionUI == null) { optionUI = FindObjectOfType<OptionUI>(); } 
        if(gameUI == null) { gameUI = FindObjectOfType<GameUI>(); } 

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                TitleUISet(true);
                OptionUISet(false);
                GameUISet(false);
                break;
            case 1:
                TitleUISet(false);
                OptionUISet(false);
                GameUISet(true);
                break;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionUISet();
            Time.timeScale = 0f;
        }
    }
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
    }  
    //==========================================================

    #region SetSceneUI
    public void TitleUISet(bool On = true)
    {
        titleUI.gameObject.SetActive(On);
    }

    public void OptionUISet(bool On = true)
    {
        optionUI.gameObject.SetActive(On);        
    }

    public void GameUISet(bool On = true)
    {
        gameUI.gameObject.SetActive(On);
    }
    #endregion

    #region GameUI

    public void UpdateScoreText(int score)
    {
        gameUI.UpdateScore(score);
    }

    public void UpdateBoxText(int box)
    {
        gameUI.UpdateBox(box);
    }
    #endregion

}
