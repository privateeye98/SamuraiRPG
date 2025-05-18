using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [Header("UI refs")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;


    [Header("Audio")]
    [SerializeField] AudioMixer mixer;
    const string KEY_RES = "ResolutionIndex";
    const string KEY_FULL = "FullScreen";
    const string KEY_BGM = "BGMVol";
    const string KEY_SFX = "SFXVol";

    Resolution[] resolutions;

    void Awake()
    {

        if (bgmSlider == null || sfxSlider == null || resolutionDropdown == null || fullscreenToggle == null)
        {
            Debug.LogError("UI 요소 연결 안됨!");
            return;
        }

        //-- 해상도 동적
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int current = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string label = $"{resolutions[i].width}×{resolutions[i].height}";
            options.Add(label);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                current = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", current);

        // - 불륨 전체화면 초기값 ----

        bgmSlider.value = PlayerPrefs.GetFloat("BGMVol", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.8f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public void ApplyAndClose()
    {
        // 1) 볼륨 즉시 적용 (BGM은 AudioMixer 권장, 예시는 단순화)
        AudioListener.volume = bgmSlider.value;         // 전체 볼륨

        // 2) 해상도/전체화면
        Resolution r = resolutions[resolutionDropdown.value];
        Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn);

        // 3) 저장
        PlayerPrefs.SetFloat("BGMVol", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        // 4) 창 닫기
        gameObject.SetActive(false);
    }
    public  void Close() => gameObject.SetActive(false); // 설정 패널 닫기
    void OnEnable()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(UpdateBGM);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(UpdateSFX);
    }
    void OnDisable()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(UpdateBGM);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(UpdateSFX);
    }

    void UpdateBGM(float v) =>
        mixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);

    void UpdateSFX(float v) =>
        mixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);

    public void OnClickSave()
    {
        GameSaveManager.I.SaveGame();
    }

    public void OnClickLoad()
    {
        GameSaveManager.I.LoadGame();
    }


}

