using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour, IListener
{
    CinemachineVirtualCamera Camera;

    public GameUI gameUI;
    bool isZoomIn = false;
    // Start is called before the first frame update
    void Start()
    {
        Camera = GetComponent<CinemachineVirtualCamera>();
        EventManager.Instance.AddListener(EVENT_TYPE.DEAD, this);   
    }

    // Update is called once per frame
    void Update()
    {
        if(isZoomIn && Input.anyKey && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            gameUI.Panel.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            OnToTalScore();
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        DOTween.To(() => Camera.m_Lens.OrthographicSize, x => Camera.m_Lens.OrthographicSize = x, 3.32f, 3)
            .OnComplete(() => OnTotalPanel());
        isZoomIn = true;
    }
    public void OnTotalPanel()
    {
        gameUI.Panel.GetComponent<Image>().DOColor(new Color(0,0,0,0.5f), 1f).OnComplete(() => OnToTalScore());
    }
    public void OnToTalScore()
    {
        gameUI.Panel.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(gameUI.GameOver());
    }
}
