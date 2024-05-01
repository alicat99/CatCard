using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverLoseUI : MonoBehaviour
{
    [SerializeField]
    RectTransform characterP;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Slider sliderOriginal;

    //temp
    [SerializeField]
    Sprite characterAnim;

    public void Activate()
    {
        StartCoroutine(DoActivate());
    }

    private IEnumerator DoActivate()
    {
        yield return new WaitForSeconds(1);

        characterP.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.OutExpo).SetLink(gameObject);
        slider.value = sliderOriginal.value;

        yield return new WaitForSeconds(2);

        slider.DOValue(0, 1).SetLink(gameObject);

        yield return new WaitForSeconds(1);

        Image characterPImage = characterP.GetComponentInChildren<Image>();
        var characterPImageRect = characterPImage.GetComponent<RectTransform>();
        characterPImageRect.DOAnchorPos(new Vector2(50, 0), 0.5f).SetEase(Ease.InExpo).SetLink(gameObject);

        yield return new WaitForSeconds(0.5f);

        characterPImageRect.DOAnchorPos(new Vector2(0, 0), 0.5f).From(new Vector2(-50, 0)).SetEase(Ease.OutExpo).SetLink(gameObject);
        characterPImage.sprite = characterAnim;
    }
}
