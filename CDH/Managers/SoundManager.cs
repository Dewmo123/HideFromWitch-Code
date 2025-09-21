using Assets._00.Work.CDH.Code.Events;
using Assets._00.Work.CDH.Code.Sound;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using DewmoLib.Utiles;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace _00.Work.CDH.Code.Managers
{

    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private EventChannelSO soundEventChannel;
        [SerializeField] private PoolItemSO soundPlayer;
        [SerializeField] private PoolManagerMono poolManager;

        private SoundPlayer bgmPlayer;

        // 노출된 파라미터 이름 (AudioMixer에서 확인한 이름과 일치해야 함)
        private const string BGM_PARAM = "BGMVolume";
        private const string SFX_PARAM = "SFXVolume";

        private void Awake()
        {
            soundEventChannel.AddListener<ReqVolumeEvent>(HandleReqVolumeEvent);
            soundEventChannel.AddListener<SetVolumeEvent>(HandleSetVolumeEvent);
            soundEventChannel.AddListener<PlaySFXEvent>(HandlePlaySFXEvent);
            soundEventChannel.AddListener<PlayBGMEvent>(HandlePlayBGMEvent);
            soundEventChannel.AddListener<StopBGMEvent>(HandleStopBGMEvent);

        }

        private void Start()
        {
            GetBGMSFXVolume();
        }

        private void GetBGMSFXVolume()
        {
            float SFXdb = PlayerPrefs.GetFloat(SFX_PARAM);
            audioMixer.SetFloat(SFX_PARAM, SFXdb);
            Debug.Log("SFX Volume : " +  SFXdb);
            float BGMdb = PlayerPrefs.GetFloat(BGM_PARAM);
            audioMixer.SetFloat(BGM_PARAM, BGMdb);
            Debug.Log("BGM Volume : " + BGMdb);
        }

        private void OnDestroy()
        {
            soundEventChannel.RemoveListener<ReqVolumeEvent>(HandleReqVolumeEvent);
            soundEventChannel.RemoveListener<SetVolumeEvent>(HandleSetVolumeEvent);
            soundEventChannel.RemoveListener<PlaySFXEvent>(HandlePlaySFXEvent);
            soundEventChannel.RemoveListener<PlayBGMEvent>(HandlePlayBGMEvent);
            soundEventChannel.RemoveListener<StopBGMEvent>(HandleStopBGMEvent);
        }

        private void HandleReqVolumeEvent(ReqVolumeEvent evt)
        {
            ResponseVolumeEvent rEvt = SoundEvents.ResponseVolumeEvent;
            float dbSfx = 0;
            float dbBgm = 0;
            audioMixer.GetFloat(SFX_PARAM,out dbSfx);
            audioMixer.GetFloat(BGM_PARAM, out dbBgm);

            rEvt.Sfx = DbToVolume(dbSfx);
            rEvt.Bgm = DbToVolume(dbBgm);
            soundEventChannel.InvokeEvent(rEvt);
        }

        private void HandlePlaySFXEvent(PlaySFXEvent evt)
        {
            SoundPlayer sfxPlayer = poolManager.Pop<SoundPlayer>(soundPlayer);
            sfxPlayer.transform.position = evt.position;
            sfxPlayer.PlaySound(evt.sound);
        }

        private void HandlePlayBGMEvent(PlayBGMEvent evt)
        {
            bgmPlayer = poolManager.Pop<SoundPlayer>(soundPlayer);
            bgmPlayer.PlaySound(evt.sound);
        }

        private void HandleStopBGMEvent(StopBGMEvent evt)
        {
            bgmPlayer?.StopSound();
        }

        private void HandleSetVolumeEvent(SetVolumeEvent evt)
        {
            float volume = evt.volume;
            float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;

            switch (evt.audioType)
            {
                case AudioTypes.SFX:
                    audioMixer.SetFloat(SFX_PARAM, db);
                    PlayerPrefs.SetFloat(SFX_PARAM, db);
                    Debug.Log("SetPlayerPrefs sound volume : " + db);
                    break;
                case AudioTypes.BGM:
                    audioMixer.SetFloat(BGM_PARAM, db);
                    PlayerPrefs.SetFloat(BGM_PARAM, db);
                    Debug.Log("SetPlayerPrefs sound volume : " + db);
                    break;
            }
        }
        private float DbToVolume(float db)
        {
            return Mathf.Pow(10f, db / 20f);
        }
    }
}