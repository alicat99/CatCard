using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardUIBackground : UISelector
{
    [SerializeField]
    CardInformation cardInformation;

    public CardData cardData { get; private set; }

    private Tween currentTween;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponent<Button>().onClick.AddListener(Deselect);
    }

    public void SelectData(ISelectable selection, CardData cardData, int intensity)
    {
        Select(selection);

        this.cardData = cardData;
        cardInformation.SetData(cardData, intensity);
    }

    protected override void OnSelectionCreate()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
        currentTween = canvasGroup.DOFade(1, 0.5f).SetLink(gameObject);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

    }

    protected override void OnSelectionEnd()
    {
        cardInformation.Close(cardData);
        cardData = null;

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
        currentTween = canvasGroup.DOFade(0, 0.5f).SetLink(gameObject);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
