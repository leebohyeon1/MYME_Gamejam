using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> uniqueResolutions;

    public TMP_Dropdown ScreenModeDropdown;
    public enum ScreenMode
    {
        FullScreenWindow = 0,
        Window = 1
    }

    int resolutionIndex;
    int screenModeIndex;


    [Header("Sound")]
    public Slider BGMSlider;
    public Slider SFXSlider;



   

    //==========================================================

    void Start()
    {
        ClearResolution();
        ClearScreenMode();

        SetInitialResolution();
        SaveOption();

   
        // �����̴� �̺�Ʈ ������ �߰�
        BGMSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SaveBtn();
        }
    }
    //==========================================================

    #region Resolution

    void ClearResolution()
    {
        Resolution[] allResolutions = Screen.resolutions;
        uniqueResolutions = allResolutions.Distinct(new ResolutionComparer()).ToList();
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            resolutionOptions.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // �ػ� �� ���� �� ����
        Resolution resolution = uniqueResolutions[resolutionIndex];
        this.resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetInitialResolution()
    {
        // ����� �ػ� ���� ������ �� ������ �����ϰ�, ������ ���� �ػ󵵷� ����
       int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // ����� ���� ���� ��� ���� ����� �ػ󵵷� ����
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            // ���� �ػ󵵸� PlayerPrefs�� ����
            for (int i = 0; i < uniqueResolutions.Count; i++)
            {
                if (uniqueResolutions[i].width == currentResolution.width && uniqueResolutions[i].height == currentResolution.height)
                {
                    this.resolutionIndex = i;
                    break;
                }
            }

            BGMSlider.value = 0.5f;
            SFXSlider.value = 0.5f;
        }
        else
        {
            // ����� ���� ���� ��� �� ������ ����
            SetResolution(savedResolutionIndex);

            // �ʱ� �����̴� �� ����
            //SetBackgroundMusicVolume(BGMSlider.value);

            //for (int i = 0; i < SoundManager.instance.sfxClips.Length; i++)
            //{
            //    SetSFXVolume(SFXSlider.value);
            //}
        }

     
    }

    #endregion

    #region ScreenMode
    void ClearScreenMode()
    {
        List<string> options = new List<string> { "FullScreen", "WindowScreen" };

        ScreenModeDropdown.ClearOptions();
        ScreenModeDropdown.AddOptions(options);
        ScreenModeDropdown.onValueChanged.AddListener(index => ChangeFullScreenMode((ScreenMode)index));

        int screenModeIndex = PlayerPrefs.GetInt("screenMode", -1);
        if (screenModeIndex == -1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            switch (screenModeIndex)
            {
                case 0:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case 1:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
                    break;
            }
        }
       
    }

    private void ChangeFullScreenMode(ScreenMode mode)
    {
        switch (mode)
        {
            case ScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                screenModeIndex = (int)ScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                screenModeIndex = (int)ScreenMode.Window;
                break;
        }
    }
    #endregion

    #region Btn
    public void SaveBtn()
    {
        SaveOption();
        Time.timeScale = 1f;
        UIManager.Instance.OptionUISet(false);
    }

    public void ResetBtn()
    {
        ResetOption();
    }

    public void ExitBtn()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
    //==========================================================

    void SaveOption()
    {
        PlayerPrefs.SetInt("resolution",resolutionIndex);
        PlayerPrefs.SetInt("screenMode", screenModeIndex);
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
       PlayerPrefs.Save();
    }

    void ResetOption()
    {
        resolutionIndex = PlayerPrefs.GetInt("resolution");
        screenModeIndex = PlayerPrefs.GetInt("screenMode");

        SetResolution(resolutionIndex);
        resolutionDropdown.value = resolutionIndex;

        ChangeFullScreenMode((ScreenMode)screenModeIndex);
        ScreenModeDropdown.value = screenModeIndex;

        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void SetBackgroundMusicVolume(float volume)
    {

        //AudioManager.instance.musicSource.volume = volume;

      
        PlayerPrefs.SetFloat("BGMVolume", volume); // ���� �� ����
    }

    public void SetSFXVolume(float volume)
    {

        //AudioManager.instance.sfxSource.volume = volume;
           
        PlayerPrefs.SetFloat("SFXVolume", volume); // ���� �� ����
    }
}

//==========================================================

public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}


