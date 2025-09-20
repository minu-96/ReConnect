using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BgmVolumeController : MonoBehaviour
{
    [Header("References")]
    public AudioMixer masterMixer;       // Master 믹서 에셋 Drag&Drop
    public Slider bgmSlider;             // UI 슬라이더 Drag&Drop

    private const string BGM_PARAM = "BGMVolume";          // Exposed Parameter 이름
    private const string PREF_KEY = "BGMVolumeLinear";   // PlayerPrefs 키

    void Awake()
    {
        // 저장된 값 불러오기 (기본값 0.8)
        float saved = PlayerPrefs.GetFloat(PREF_KEY, 0.8f);
        if (bgmSlider != null) bgmSlider.value = saved;
        SetBgmVolume(saved);

        // 슬라이더 변화 이벤트 연결
        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(SetBgmVolume);
    }

    public void SetBgmVolume(float linear)
    {
        // 0~1 선형 값을 dB 스케일로 변환 (-80dB ~ 0dB)
        float clamped = Mathf.Clamp(linear, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;

        masterMixer.SetFloat(BGM_PARAM, dB);
        PlayerPrefs.SetFloat(PREF_KEY, linear);
    }
}
