using UnityEngine;
using TMPro;
using System;

// credits to https://youtu.be/YUIohCXt_pc
public class Tooltip : MonoBehaviour
{

    [SerializeField]
    private RectTransform _canvas;

    private RectTransform _background;
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;

    // allows dynamic tooltips apparently...
    private Func<string> _getTooltipText;

    private void Awake()
    {
        _background = transform.Find("Background").GetComponent<RectTransform>();
        _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _rectTransform = transform.GetComponent<RectTransform>();

        HideTooltip();
    }

    private void Update()
    {
        SetText(_getTooltipText());

        Vector2 pos = Input.mousePosition / _canvas.localScale.x;

        if (pos.x + _background.rect.width > _canvas.rect.width)
        {
            pos.x = _canvas.rect.width - _background.rect.width;
        }

        if (pos.y + _background.rect.height > _canvas.rect.height)
        {
            pos.y = _canvas.rect.height - _background.rect.height;
        }

        _rectTransform.anchoredPosition = pos;
    }

    private void SetText(string tooltipText)
    {
        _text.SetText(tooltipText);
        _text.ForceMeshUpdate();

        Vector2 textSize = _text.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        _background.sizeDelta = textSize + padding;
    }

    public void ShowTooltip(string tooltipText)
    {
        ShowTooltip(() => tooltipText);
    }

    public void ShowTooltip(Func<string> getTooltipText)
    {
        this._getTooltipText = getTooltipText;
        gameObject.SetActive(true);
        SetText(getTooltipText());
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

}
