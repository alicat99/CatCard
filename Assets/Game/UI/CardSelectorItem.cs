using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CardSelectorItem : MonoBehaviour, ISelectable
{
    public CardData cardData { get; private set; }

    [SerializeField]
    Image icon;
    [SerializeField]
    Image selectionHighlighter;
    [SerializeField]
    TextMeshProUGUI intensityText;
    [SerializeField]
    Image intensityTextBG;
    public CardSelector parent { get; private set; }

    private Button button;
    private Tween currentTween;
    private bool isSelected = false;

    private bool _highlight = false;
    public bool highlight
    {
        get
        {
            return _highlight;
        }
        set
        {
            if (value != _highlight)
            {
                if (value)
                {
                    if (currentTween != null && currentTween.IsActive())
                    {
                        currentTween.Kill();
                    }
                    selectionHighlighter.enabled = true;
                    currentTween = selectionHighlighter.transform.DOScale(Vector3.one * 1.5f, 0.3f).From(Vector3.one).SetLink(gameObject).SetEase(Ease.OutQuint);
                }
                else
                {
                    if (currentTween != null && currentTween.IsActive())
                    {
                        currentTween.Kill();
                    }
                    currentTween = selectionHighlighter.transform.DOScale(0, 0.5f).SetEase(Ease.InQuad).SetLink(gameObject).OnComplete(()=>
                    {
                        selectionHighlighter.enabled = false;
                    });
                }
            }
            _highlight = value;
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (!isSelected)
                parent.background.SelectData(this, cardData, cardData.intensity);
            else
                parent.background.Deselect();
        });
    }

    public void SetData(CardData cardData, CardSelector parent)
    {
        this.cardData = cardData;
        this.parent = parent;
        icon.sprite = cardData.icon;
        if (cardData.intensity == 0)
        {
            intensityText.enabled = false;
            intensityTextBG.enabled = false;
        }
        else
        {
            intensityText.enabled = true;
            intensityText.text = cardData.intensity.ToString();
            intensityTextBG.enabled = true;
        }

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
        selectionHighlighter.enabled = false;
        _highlight = false;
        isSelected = false;
    }

    public void OnSelect()
    {
        highlight = true;
        isSelected = true;

        parent.selectedItem = this;
    }

    public void OnDeselect()
    {
        highlight = false;
        isSelected = false;

        if (parent.selectedItem == this)
            parent.selectedItem = null;
    }
}
