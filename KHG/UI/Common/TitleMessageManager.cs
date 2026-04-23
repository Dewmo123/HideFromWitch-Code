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
    private Sequence _currentSequence;
    private TMP_Text _currentText;

    private void Awake()
    {
        InitUI();
        uiChannel.AddListener<MessageEvent>(HandleMessage);
        packetChannel.AddListener<MessageEvent>(HandleMessage);
    }

    private void OnDestroy()
    {
        StopCurrentMessage();
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
        TitleData data = evt.Data;
        string msg = evt.Message;
        TMP_Text targetTmp = ResolveTargetText(data.mode);

        StopCurrentMessage();

        targetTmp.gameObject.SetActive(true);
        Showing = true;

        targetTmp.color = new Color(1, 1, 1, 0);
        targetTmp.SetText(msg);

        _currentText = targetTmp;
        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(targetTmp.DOFade(1, data.fadeInTime));
        _currentSequence.AppendInterval(data.lifeTime);
        _currentSequence.Append(targetTmp.DOFade(0, data.fadeOutTime))
            .OnComplete(() =>
            {
                targetTmp.gameObject.SetActive(false);
                Showing = false;
                _currentText = null;
                _currentSequence = null;
            });
    }

    private TMP_Text ResolveTargetText(TitleMode mode)
    {
        return mode == TitleMode.MainTitle ? mainTitle : subTitle;
    }

    private void StopCurrentMessage()
    {
        if (_currentSequence != null && _currentSequence.IsActive())
            _currentSequence.Kill();

        if (_currentText != null)
            _currentText.gameObject.SetActive(false);

        Showing = false;
        _currentText = null;
        _currentSequence = null;
    }

    public void Initialize(ICoreUIContext coreUIContext)
    {
    }
}
