using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite trueSprite;
    [SerializeField] private Sprite falseSprite;
    public UnityEvent<bool> ToggleEvent;

    private bool _value;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SetImage();
    }

    public void Switch()
    {
        _value = !_value;

        ToggleEvent.Invoke(_value);
        SetImage();
    }

    private void SetImage()
    {
        targetImage.sprite = _value ? trueSprite : falseSprite;
    }
}
