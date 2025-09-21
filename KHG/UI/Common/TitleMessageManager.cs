using DewmoLib.Utiles;
using DG.Tweening;
using KHG.Events;
using KHG.UIs;
using TMPro;
using UnityEngine;

public class TitleMessageManager : MonoBehaviour, ICoreUI
{
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private EventChannelSO packetChannel;

    [SerializeField] private TMP_Text mainTitle;
    [SerializeField] private TMP_Text subTitle;

    public bool Showing { get; private set; }

    private void Awake()
    {
        InitUI();
        uiChannel.AddListener<MessageEvent>(HandleMessage);
        packetChannel.AddListener<MessageEvent>(HandleMessage);
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<MessageEvent>(HandleMessage);
        packetChannel.RemoveListener<MessageEvent>(HandleMessage);
    }

    private void InitUI()
    {
        mainTitle.gameObject.SetActive(false);
        subTitle.gameObject.SetActive(false);
    }

    private void HandleMessage(MessageEvent evt)
    {
        print("타이틀 이벤트 받음:" + evt.Message);
        if (Showing == true) RestartMessage(evt);

        TMP_Text targetTmp = mainTitle;

        TitleData data = evt.Data;
        string msg = evt.Message;

        switch (data.mode)
        {
            case TitleMode.MainTitle:
                targetTmp = mainTitle;
                break;
            case TitleMode.SubTitle:
                targetTmp = subTitle;
                break;
        }

        targetTmp.gameObject.SetActive(true);
        Showing = true;

        targetTmp.color = new Color(1, 1, 1, 0);
        targetTmp.SetText(msg);

        Sequence seq = DOTween.Sequence();
        seq.Append(targetTmp.DOFade(1, data.fadeInTime));
        seq.AppendInterval(data.lifeTime);
        seq.Append(targetTmp.DOFade(0, data.fadeOutTime)).OnComplete(() => { targetTmp.gameObject.SetActive(false); Showing = false; });
    }

    private void RestartMessage(MessageEvent evt)
    {
        DOTween.Kill(this);
        print("타이틀 제공 겹침으로 인해 이전 타이틀이 제거됩니다.");
        Showing = false;

        HandleMessage(evt);
    }

    public void Initialize(CoreUI coreUI)
    {
    }
}
