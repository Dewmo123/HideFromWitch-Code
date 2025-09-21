using Assets._00.Work.CDH.Code.Events;
using Assets._00.Work.CDH.Code.Sound;
using Assets._00.Work.YHB.Scripts.Others;
using DewmoLib.Utiles;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingApplier : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Slider sensitiveSlider;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    [SerializeField] private ToggleButton horrorToggle;
    [SerializeField] private bool mouseLock = true;
    [Space]
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private CameraRotateValueSO camSO;

    private static readonly string _SENSIVILITY_PRAFS = "UserSensivility";


    private void Start()
    {
        InitSetting();

        soundChannel.AddListener<ResponseVolumeEvent>(HandleResponseVolumeEvent);
    }

    public void UiEnable()
    {
        soundChannel.InvokeEvent(SoundEvents.ReqVolumeEvent);
        
    }

    private void OnDestroy()
    {
        soundChannel.RemoveListener<ResponseVolumeEvent>(HandleResponseVolumeEvent);
    }

    private void HandleResponseVolumeEvent(ResponseVolumeEvent evt)
    {
        float sensivility = PlayerPrefs.GetFloat(_SENSIVILITY_PRAFS, 0.17f);
        SetSettingSliders(evt.Sfx,evt.Bgm, sensivility);
    }

    public void SetSettingPanel(bool value)
    {
        panel.SetActive(value);
        if (value) InitSetting();
    }
    public void InitSetting()
    {
        horrorToggle.CurrentValue = true;//�ݴ밪
        horrorToggle.Refresh();
    }
    public void OnApply()
    {
        SetVolumeEvent evt = SoundEvents.SetVolumeEvent;

        evt.audioType = AudioTypes.SFX;
        evt.volume = sfxSlider.value;
        soundChannel.InvokeEvent(evt);

        evt.audioType = AudioTypes.BGM;
        evt.volume = bgmSlider.value;
        soundChannel.InvokeEvent(evt);

        camSO.rotationSensitivity = sensitiveSlider.value;
        PlayerPrefs.SetFloat(_SENSIVILITY_PRAFS, sensitiveSlider.value);
    }

    public void OnCancel()
    {
        SetSettingPanel(false);
        if (mouseLock == false) return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetSettingSliders(float s,float b,float c)
    {
        sfxSlider.value = s;
        bgmSlider.value = b;
        sensitiveSlider.value = c;

        camSO.rotationSensitivity = c;
    }
}
