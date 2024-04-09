using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIEffectBubble : MonoBehaviour
{
    [SerializeField]
    RectTransform shadow;
    [SerializeField]
    Image icon;
    [SerializeField]
    RectTransform number;
    [SerializeField]
    TextMeshProUGUI numberText;
    [SerializeField]
    TextMeshProUGUI descriptionText;
    [SerializeField]
    RectTransform descriptionBG;

    new RectTransform transform;
    CanvasGroup canvasGroup;

    float duration = 0;
    public bool isLive { get; private set; } = false;
    Coroutine life;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(Vector2 pos, Transform parent, Sprite sprite, int intensity, string description)
    {
        transform.SetParent(parent);
        transform.position = pos;
        transform.localScale = Vector3.one;

        shadow.DOAnchorPos(new Vector2(0, 150), 1).From(Vector2.zero).SetLink(gameObject).SetEase(Ease.OutExpo);
        shadow.DORotate(new Vector3(0, 0, 315), 1, RotateMode.FastBeyond360).From(Vector3.zero).SetLink(gameObject).SetEase(Ease.OutExpo);

        icon.sprite = sprite;
        icon.rectTransform.DOAnchorPos(new Vector2(0, 330), 1).From(new Vector2(0, 50)).SetLink(gameObject).SetEase(Ease.OutExpo);

        if (intensity == 0)
        {
            number.gameObject.SetActive(false);
        }
        else
        {
            number.gameObject.SetActive(true);
            numberText.text = intensity.ToString();
            number.DOAnchorPos(new Vector2(0, 150), 1).From(new Vector2(0, -150)).SetLink(gameObject).SetEase(Ease.OutExpo);
        }

        descriptionText.text = Utils.GetLocalizedString(description, "EffectBubble");
        //descriptionText.DOColor(new Color(0, 0, 0, 0), 1.5f).From(new Color(0, 0, 0, 1)).SetLink(gameObject);
        DOTween.To(() => descriptionText.characterSpacing, value =>
        {
            descriptionText.characterSpacing = value;
        }, 30, 1).From(0).SetLink(gameObject).SetEase(Ease.OutExpo);

        descriptionText.characterSpacing = 30;
        float width = descriptionText.preferredWidth + 40;
        descriptionBG.sizeDelta = new Vector2(width, descriptionBG.sizeDelta.y);

        descriptionBG.DOAnchorPos(new Vector2(0, -70), 2).From(new Vector2(0, -100)).SetLink(gameObject).SetEase(Ease.OutExpo);

        duration = 0.7f;
        life = StartCoroutine(Life());
    }

    IEnumerator Life()
    {
        isLive = true;
        canvasGroup.DOFade(1, 0.5f).From(0).SetLink(gameObject).SetEase(Ease.OutBounce);

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        isLive = false;
        canvasGroup.DOFade(0, 0.5f).SetLink(gameObject).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(0.6f);
        ObjectPoolManager.Instance.Despawn(gameObject);

        life = null;
    }

    public void SetIntensity(int intensity)
    {
        if (intensity == 0)
        {
            number.gameObject.SetActive(false);
        }
        else
        {
            number.gameObject.SetActive(true);
            numberText.text = intensity.ToString();
        }
        canvasGroup.DOFade(1, 0.5f).From(0f).SetLink(gameObject);
    }

    public void AddDuration(float t = 0.5f)
    {
        duration += t;
    }

    private void OnDisable()
    {
        if (life != null)
        {
            isLive = false;
            StopCoroutine(life);
        }
    }
}
