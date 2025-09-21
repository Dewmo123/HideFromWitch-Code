using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility.Unity.Common;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    [SerializeField] private RectTransform toggleImage;
    [SerializeField] private TextMeshProUGUI toggleText;
    [SerializeField] private Vector3 offVector;
    [SerializeField] private Vector3 onVector;
    [SerializeField] private float imageMoveSpeed = 0.3f;
    public UnityEvent<bool> ToggleEvent;
    public bool CurrentValue { get; set; } = false;

    private Button _button;

    public void Refresh() => HandleToggleClicked();

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleToggleClicked);
    }

    private void HandleToggleClicked()
    {
        CurrentValue = !CurrentValue;
        ToggleEvent?.Invoke(CurrentValue);
        toggleImage.DOAnchorPos(GetImageTransform(), imageMoveSpeed);
        toggleText.SetText(CurrentValue ? "ÄÑÁü" : "²¨Áü");
    }

    private Vector3 GetImageTransform()
    {
        return CurrentValue ? onVector : offVector;
    }
}
