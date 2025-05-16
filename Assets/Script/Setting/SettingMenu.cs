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

        //-- �ػ� ����
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int current = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string label = $"{resolutions[i].width}��{resolutions[i].height}";
            options.Add(label);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                current = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", current);

        // - �ҷ� ��üȭ�� �ʱⰪ ----

        bgmSlider.value = PlayerPrefs.GetFloat("BGMVol", 0.8f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.8f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public void ApplyAndClose()
    {
        // 1) ���� ��� ���� (BGM�� AudioMixer ����, ���ô� �ܼ�ȭ)
        AudioListener.volume = bgmSlider.value;         // ��ü ����

        // 2) �ػ�/��üȭ��
        Resolution r = resolutions[resolutionDropdown.value];
        Screen.SetResolution(r.width, r.height, fullscreenToggle.isOn);

        // 3) ����
        PlayerPrefs.SetFloat("BGMVol", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        // 4) â �ݱ�
        gameObject.SetActive(false);
    }
    public  void Close() => gameObject.SetActive(false); // ���� �г� �ݱ�
    void OnEnable()
    {
        bgmSlider.onValueChanged.AddListener(UpdateBGM);
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
    }
    void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(UpdateBGM);
        sfxSlider.onValueChanged.RemoveListener(UpdateSFX);
    }

    void UpdateBGM(float v) =>
        mixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);

    void UpdateSFX(float v) =>
        mixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
}

