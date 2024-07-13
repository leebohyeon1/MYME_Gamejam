using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class GameUI : MonoBehaviour
{
    public TMP_Text scoreText, BoxText, scoreText2, BoxText2, totalScoreText, CountText;
    public GameObject Panel;
    public Button[] Btn;
    public Camera mainCamera;
    public Image[] indicator;
    public GameObject NameBar;
    public TMP_InputField nameInput;

    private const string ScorePrefix = "Score: ";
    private const string BoxPrefix = "Box: ";
    private int score, box;
    private float totalScore;
    private float curNum, scoreTimer, timer;
    private bool isTotalCalculated;
    private bool isSound;
    private bool isScore;
    private int Count = 3;
    private int index = 0;
    string nameT;

    private void Awake()
    {
        nameT = nameInput.GetComponent<TMP_InputField>().text;
    }

    private void Start()
    {
        isSound = false;
        isScore = false;
        mainCamera = Camera.main;
        UpdateUI();

        if (PlayerPrefs.GetInt("Count", 0) == 1)
        {
            CountText.gameObject.SetActive(true);
            GameManager.Instance.isCount = true;
            StartCoroutine(CountDown());
        }
    }

    private void Update()
    {
        if (isTotalCalculated && AnyKeyExceptWASD())
        {
            ResetScale(scoreText2.rectTransform);
            ResetScale(BoxText2.rectTransform);

            isTotalCalculated = false;
            isScore = true;
            scoreTimer = 4f;
            curNum = totalScore;
            HandleBestScoreDisplay();
            StartCoroutine(SetButtons(true));
        }

        if (isTotalCalculated)
        {
            HandleTotalScoreCalculation();
        }

        UpdateBoxIndicators();

        if (nameInput.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();

        }
        if (nameInput.gameObject.activeSelf)
        {
            return;
        }
        HandleButtonInput();
    }

    private void HandleBestScoreDisplay()
    {
        if (totalScore > GameManager.Instance.BestScore)
        {
            totalScoreText.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f).SetEase(Ease.InOutBack);
            NameBar.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.BestScore);
        }
        else
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.JustScore);
        }
        Debug.Log(curNum);
        totalScoreText.text = curNum.ToString();
        AudioManager.instance.StopSfx(AudioManager.Sfx.Slot);
    }

    private void HandleTotalScoreCalculation()
    {
        scoreTimer += Time.deltaTime;
        if (scoreTimer > 0.9f && !isSound && !isScore)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Slot);
            isSound = true;
        }

        if (scoreTimer > 1.4f)
        {
            if (totalScore > GameManager.Instance.BestScore)
            {
                totalScoreText.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f).SetEase(Ease.InOutBack);
            }

            if (scoreTimer > 2f)
            {
                Debug.Log(totalScore);
                totalScoreText.text = totalScore.ToString();
                isTotalCalculated = false;
                HandleBestScoreDisplay();
                return;
            }

            UpdateTotalScoreUI();
        }
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
        float multiplier = 1f;

        // Adjust multiplier based on zombie count
        switch (GameManager.Instance.ZombieList.Count)
        {
            case 5:
                multiplier = 1.25f;
                break;
            case 10:
                multiplier = 1.5f;
                break;
            case 15:
                multiplier = 2f;
                break;
        }

        totalScore = (box == 0 ? 1 : box) * score * multiplier;
        scoreText2.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.4f);
        BoxText2.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.5f);

        totalScoreText.text = totalScore.ToString();
        if (totalScore > GameManager.Instance.BestScore)
        {
            NameBar.SetActive(true);
            // Assuming you handle BestScore display elsewhere
        }

        StartCoroutine(SetButtons(true));
    }
    public IEnumerator SetButtons(bool active)
    {
        yield return new WaitForSeconds(1f);
        foreach (var button in Btn)
        {
            button.gameObject.SetActive(active);
        }

        index = 0;
        Btn[0].gameObject.transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
    }

    public void RetryBtn()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("Count", 0);
    }

    public void ExitBtn()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateUI()
    {
        scoreText.text = ScorePrefix + score;
        BoxText.text = BoxPrefix + box;

        foreach (var button in Btn)
        {
            button.gameObject.SetActive(false);
        }
    }

    public IEnumerator CountDown()
    {
        for (int i = 0; i < 3; i++)
        {
            CountText.text = Count.ToString();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.CountDown);
            yield return new WaitForSeconds(1f);
            Count--;
        }
        CountText.gameObject.SetActive(false);
        GameManager.Instance.isCount = false;
    }

    private bool AnyKeyExceptWASD()
    {
        return Input.anyKey && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D);
    }

    private void ResetScale(RectTransform rectTransform)
    {
        rectTransform.localScale = Vector3.one;
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

    private void HandleButtonInput()
    {
        if (Btn[index].gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                index = 0;
                Btn[0].gameObject.transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
                Btn[1].gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                index = 1;
                Btn[1].gameObject.transform.DOScale(transform.localScale * 1.3f, 0.25f).SetEase(Ease.InQuad);
                Btn[0].gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InQuad);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (index)
                {
                    case 0:
                        RetryBtn();
                        break;
                    case 1:
                        ExitBtn();
                        break;
                }
            }
        }
    }

    public void InputName()
    {
        nameT = nameInput.text;
        PlayerPrefs.SetString("BestPlayer", nameT);
        PlayerPrefs.SetFloat("BestScore", totalScore);
        GameManager.Instance.ScoreSet(totalScore, nameT);
        NameBar.SetActive(false);
    }
}