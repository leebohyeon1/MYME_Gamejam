using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public TMP_Text Press;
    public TMP_Text LoadingText;

    private bool isLoad = false;
    private float timer = 0f;
    private float alpha = 0f;

    private void Awake()
    {
        isLoad = false;
    }

    void Start()
    {
        Press.color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (!isLoad)
        {
            UpdateTimers();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene();
        }
    }

    private void UpdateTimers()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            LoadingText.gameObject.SetActive(false);
            FadeInPressText();
        }
    }

    private void FadeInPressText()
    {
        alpha += Time.deltaTime * 0.5f; // 매 프레임마다 0.5씩 증가

        Press.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 1)); // alpha 값 클램프

        if (alpha >= 1)
        {
            EnablePress();
        }
    }

    private void EnablePress()
    {
        isLoad = true;
        Press.gameObject.SetActive(true);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("Count", 1);
    }
}