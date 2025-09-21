using KHG.UIs;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour, ICoreUI
{
    [SerializeField] private TMP_Text timeTmp;

    public void Initialize(CoreUI coreUI)
    {
    }

    public void SetTime(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);

        string fSec = sec >= 10 ? sec.ToString() : $"0{sec}";

        string fTime = $"{min}:{fSec}";
        timeTmp.SetText(fTime);
    }
}
