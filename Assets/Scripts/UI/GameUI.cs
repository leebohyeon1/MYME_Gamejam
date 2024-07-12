using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text scoreText, BoxText, scoreText2, BoxText2, totalScoreText;
    public GameObject Panel;
    public Button[] Btn;
    public Camera mainCamera;
    public Image[] indicator;

    private const string ScorePrefix = "Score: ";
    private const string BoxPrefix = "Box: ";
    private int score, box;
    private float totalScore;
    private float curNum, scoreTimer, timer;
    private bool isTotalCalculated;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateUI();
    }

    private void Update()
    {
        if (isTotalCalculated && AnyKeyExceptWASD())
        {
            ResetScale(scoreText2.rectTransform);
            ResetScale(BoxText2.rectTransform);

            isTotalCalculated = false;
            scoreTimer = 4f;
            curNum = totalScore;
            totalScoreText.text = curNum.ToString();
            SetButtons(true);
        }

        if (isTotalCalculated)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer > 0.9f)
            {
                if (scoreTimer > 2f)
                {
                    totalScoreText.text = totalScore.ToString();
                    isTotalCalculated = false;
                    return;
                }

                UpdateTotalScoreUI();
            }
        }
        UpdateBoxIndicators();
    }

    private void UpdateTotalScoreUI()
    {
        timer += Time.deltaTime;
        if (timer > 0.05f)
        {
            totalScoreText.text = GenerateRandomNumbers(totalScore.ToString().Length);
            timer = 0f;
        }
    }

    private string GenerateRandomNumbers(int length)
    {
        string randomNumbers = "";
        for (int i = 0; i < length; i++)
        {
            randomNumbers += Random.Range(0, 10).ToString();
        }
        return randomNumbers;
    }

    private bool AnyKeyExceptWASD()
    {
        return Input.anyKey && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D);
    }

    private void ResetScale(RectTransform rectTransform)
    {
        rectTransform.localScale = Vector3.one;
    }

    private void UpdateBoxIndicators()
    {
        for (int i = 0; i < GameManager.Instance.boxList.Count; i++)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(GameManager.Instance.boxList[i].transform.position);
            bool isOffScreen = screenPoint.x <= 0 || screenPoint.x >= 1 || screenPoint.y <= 0 || screenPoint.y >= 1;
            indicator[i].gameObject.SetActive(isOffScreen);

            if (isOffScreen)
            {
                UpdateIndicatorPosition(screenPoint, i);
            }
        }
    }

    private void UpdateIndicatorPosition(Vector3 screenPoint, int index)
    {
        screenPoint.x = Mathf.Clamp(screenPoint.x, 0.05f, 0.95f);
        screenPoint.y = Mathf.Clamp(screenPoint.y, 0.05f, 0.95f);
        screenPoint.z = 0;

        Vector3 screenPosition = mainCamera.ViewportToScreenPoint(screenPoint);
        indicator[index].transform.position = screenPosition;

        Vector3 direction = (mainCamera.ScreenToWorldPoint(screenPosition) - mainCamera.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        indicator[index].rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = ScorePrefix + score;
    }

    public void UpdateBox(int box)
    {
        this.box = box;
        BoxText.text = BoxPrefix + box;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        isTotalCalculated = true;
        yield return new WaitForSeconds(0.1f);
        scoreText2.text = scoreText.text;
        BoxText2.text = BoxText.text;
        totalScore = (box == 0 ? 1 : box) * score;
        scoreText2.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.4f);
        BoxText2.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.5f);

        SetButtons(true);
    }

    private void AnimateScoreText()
    {
       
        BoxText2.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
    }

    public void SetButtons(bool active)
    {
        foreach (var button in Btn)
        {
            button.gameObject.SetActive(active);
        }
    }

    public void RetryBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitBtn()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateUI()
    {
        // 초기 점수와 박스 수를 UI에 설정합니다.
        scoreText.text = "Score: " + score;
        BoxText.text = "Box: " + box;

        // 버튼과 기타 UI 컴포넌트들의 초기 상태를 설정할 수 있습니다.
        foreach (var button in Btn)
        {
            button.gameObject.SetActive(false);  // 게임 시작 시 버튼을 숨깁니다.
        }
    }
}