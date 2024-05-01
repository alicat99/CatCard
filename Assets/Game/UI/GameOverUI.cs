using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    GameOverWinUI winUI;
    [SerializeField]
    GameOverLoseUI loseUI;

    private CanvasGroup canvasGroup;
    private Button button;
    private Image image;

    private CardUIBackground background;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        button = GetComponent<Button>();
        button.interactable = false;

        background = GameManager.Instance.card.background;
        button.onClick.AddListener(background.Deselect);

        image = GetComponent<Image>();
        image.color = image.color.SetA(0);

        winUI.gameObject.SetActive(false);
        loseUI.gameObject.SetActive(false);
    }

    private void Activate()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        button.interactable = true;

        image.DOFade(0.7f, 1).SetLink(gameObject);
    }

    public void OnWin()
    {
        Activate();

        winUI.gameObject.SetActive(true);
        winUI.Activate();
    }

    public void OnLose()
    {
        Activate();

        loseUI.gameObject.SetActive(true);
        loseUI.Activate();
    }
}
