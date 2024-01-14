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

        if (!open)
        {
            if (currentTween != null && currentTween.IsActive())
                currentTween.Kill();
            currentTween = rectTransform.DOAnchorPosY(100, 0.5f).SetEase(Ease.OutCubic).SetLink(gameObject);
            open = true;
        }

        this.cardData = cardData;
    }

    public void Close(CardData cardData)
    {
        if (this.cardData != cardData)
            return;

        if (open)
        {
            if (currentTween != null && currentTween.IsActive())
                currentTween.Kill();
            currentTween = rectTransform.DOAnchorPosY(700, 0.5f).SetEase(Ease.InQuad).SetLink(gameObject);
            open = false;
        }
    }
}
