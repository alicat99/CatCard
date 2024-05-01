using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CardInformation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI cardName;
    [SerializeField]
    TextMeshProUGUI cardDescription;

    RectTransform rectTransform;
    CardData cardData;
    bool open = false;

    Tween currentTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetData(CardData cardData, int intensity)
    {
        cardName.text = Utils.GetLocalizedString(cardData.cardName);
        var text = Utils.GetLocalizedString(cardData.description);
        text = text.Replace("$N", $"<#FF62B2>{intensity}</color>");
        cardDescription.text = text;

        var height = Mathf.Clamp(cardDescription.preferredHeight + 350, 600, 800);
        if (!open)
        {
            open = true;
            Animate(open, height);
        }
        else if (Mathf.Abs(rectTransform.sizeDelta.y - height) > 3)
        {
            Animate(open, height);
        }

        this.cardData = cardData;
    }

    public void Close(CardData cardData)
    {
        if (this.cardData != cardData)
            return;

        if (open)
        {
            open = false;
            Animate(open);
        }
    }

    private void Animate(bool open, float height = 600)
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        var seq = DOTween.Sequence()
            .Join(
                rectTransform.DOSizeDelta(rectTransform.sizeDelta.SetY(height), 0.5f).SetEase(Ease.OutCubic).SetLink(gameObject)
            );
        
        if (open)
        {
            seq.Join(
                rectTransform.DOAnchorPosY(100, 0.5f).SetEase(Ease.OutCubic).SetLink(gameObject)
            );
        }
        else
        {
            seq.Join(
                rectTransform.DOAnchorPosY(700, 0.5f).SetEase(Ease.InQuad).SetLink(gameObject)
            );
        }

        currentTween = seq;
    }
}
