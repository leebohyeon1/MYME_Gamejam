using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class TitleUI : MonoBehaviour
{
    public GameObject title;
    public TMP_Text nameText;
    public TMP_Text score;
    public GameObject Loading;
    public GameObject[] Btn;

    private int BtnIndex = 0;

    void Start()
    {
        AnimateTitle();
        HighlightButton(BtnIndex);
        LoadPlayerData();
    }

    void Update()
    {
        if (UIManager.Instance.optionUI.gameObject.activeSelf)
        {
            return;
        }

        HandleInput();
    }

    private void AnimateTitle()
    {
        title.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 2f, 2, 0).SetEase(Ease.InCubic);
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("BestPlayer"))
        {
            GameManager.Instance.BestPlayer = PlayerPrefs.GetString("BestPlayer");
            nameText.text = GameManager.Instance.BestPlayer;
            GameManager.Instance.BestScore = PlayerPrefs.GetFloat("BestScore");
            score.text = GameManager.Instance.BestScore.ToString();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            NavigateButtons(1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            NavigateButtons(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateButton();
        }
    }

    private void NavigateButtons(int direction)
    {
        BtnIndex = Mathf.Clamp(BtnIndex + direction, 0, Btn.Length - 1);
        UpdateButtonScales();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Btn);
    }

    private void UpdateButtonScales()
    {
        for (int i = 0; i < Btn.Length; i++)
        {
            if (i == BtnIndex)
            {
                HighlightButton(i);
            }
            else
            {
                ResetButtonScale(i);
            }
        }
    }

    private void HighlightButton(int index)
    {
        Btn[index].transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
    }

    private void ResetButtonScale(int index)
    {
        Btn[index].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
    }

    private void ActivateButton()
    {
        switch (BtnIndex)
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
