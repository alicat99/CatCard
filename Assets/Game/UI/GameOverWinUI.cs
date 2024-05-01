using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverWinUI : MonoBehaviour
{
    [SerializeField]
    RectTransform characterP;
    [SerializeField]
    RectTransform characterE;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Slider sliderOriginal;
    [SerializeField]
    RectTransform button;

    //temp
    [SerializeField]
    Sprite characterAnim;

    [SerializeField]
    GameOverTransitionUI transition;

    private bool isButtonClicked = false;

    public void Activate()
    {
        button.GetComponent<Button>().onClick.AddListener(() => isButtonClicked = true);

        StartCoroutine(DoActivate());
    }

    private IEnumerator DoActivate()
    {
        yield return new WaitForSeconds(1);

        characterP.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.OutExpo).SetLink(gameObject);
        characterE.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.OutExpo).SetLink(gameObject);

        button.DOAnchorPosX(0, 2).SetEase(Ease.OutExpo).SetLink(gameObject);

        slider.value = sliderOriginal.value;

        yield return new WaitUntil(() => isButtonClicked);

        characterP.GetComponentInChildren<Image>().sprite = characterAnim;
        characterP.DOShakePosition(0.5f, 50);

        slider.DOValue(0, 1).SetLink(gameObject);

        yield return new WaitForSeconds(1);

        characterE.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);

        transition.gameObject.SetActive(true);
        transition.Activate();
        transition.CreateOrb(characterE.position);
    }
}
