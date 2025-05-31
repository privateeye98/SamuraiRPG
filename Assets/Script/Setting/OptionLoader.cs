using UnityEngine;
using UnityEngine.Audio;

public class OptionLoader : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    void Awake()
    {
        // PlayerPrefs 키 상수 재사용
        float bgm = PlayerPrefs.GetFloat("BGMVol", 0.8f);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 0.8f);
        mixer.SetFloat("BGM", Mathf.Log10(bgm) * 20f);
        mixer.SetFloat("SFX", Mathf.Log10(sfx) * 20f);

        int resIdx = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool fScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        Resolution[] list = Screen.resolutions;
        resIdx = Mathf.Clamp(resIdx, 0, list.Length - 1);
        Screen.SetResolution(list[resIdx].width, list[resIdx].height, fScreen);

        // 품질(Quality) 옵션 추가했다면 여기서 QualitySettings.SetQualityLevel()
    }
}
