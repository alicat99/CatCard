using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using ActSystem;

public class CardFieldItem : MonoBehaviour, ISelectable
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Image highlight;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    TextMeshProUGUI intensityText;
    [SerializeField]
    Image intensityTextBG;

    public CardField parent { get; private set; }
    public int fieldType { get; private set; }
    public Vector2Int pos { get; private set; }

    public CardInstance cardInstance { get; private set; }
    public CardInstance systemInstance { get; private set; }

    private bool visibility
    {
        get => _visibility;
        set
        {
            if (_visibility != value)
            {
                _visibility = value;
                if (visibilityTween != null && visibilityTween.IsActive())
                {
                    visibilityTween.Kill();
                }
                visibilityTween = icon.DOFade(_visibility ? 1 : 0, 0.5f).SetEase(Ease.OutBounce).SetLink(gameObject);
            }
        }
    }
    private bool _visibility;
    private Tween visibilityTween;

    private Tween currentFadeTween = null;

    public void Initialize(CardField parent, int fieldType, Vector2Int pos)
    {
        this.parent = parent;
        GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponent<Image>().sprite = sprites[fieldType];
        this.fieldType = fieldType;
        this.pos = pos;
        cardInstance = null;
        var dummy = GameManager.Instance.card.dummyCardData;
        systemInstance = new CardInstance(pos, dummy, EntityType.P, parent);
    }

    private void OnClick()
    {
        if (cardInstance == null)
        {
            CardSelector cardSelector = parent.cardSelector;
            if (cardSelector.selectedItem == null || parent.isRunning)
            {
                parent.background.Deselect();
                return;
            }

            var newCardData = cardSelector.selectedItem.cardData;
            cardSelector.PopCard(cardSelector.selectedItem);
            SetData(newCardData, EntityType.P);
            parent.background.Deselect();
        }
        else
        {
            parent.background.SelectData(this, cardInstance.card, cardInstance.intensity);
        }
    }

    public void SetData(CardData cardData, EntityType type)
    {
        if (cardData == null)
        {
            cardInstance = null; 
            visibility = false;
            intensityText.enabled = false;
            intensityTextBG.enabled = false;
            return;
        }

        cardInstance = new CardInstance(pos, cardData, type, parent);
        UpdateUI();
    }

    public void UpdateUI()
    {
        visibility = true;
        icon.sprite = cardInstance.card.icon;
        var c = cardInstance.entityType == EntityType.P ? Color.white : Color.red;
        c.a = icon.color.a;
        icon.color = c;

        int intensity = cardInstance.intensity;
        if (intensity == 0)
        {
            intensityText.enabled = false;
            intensityTextBG.enabled = false;
        }
        else
        {
            intensityText.enabled = true;
            intensityText.text = intensity.ToString();
            intensityTextBG.enabled = true;
        }
    }

    private void ActivationAnim(Vector2Int dir)
    {
        SetArrowDir(dir);

        currentFadeTween = highlight.DOFade(0, 1).From(1).SetLink(gameObject);
    }

    private void SetArrowDir(Vector2Int dir)
    {
        float r;
        if (dir == Vector2Int.right)
            r = 0;
        else if (dir == Vector2Int.left)
            r = 180;
        else if (dir == Vector2Int.up)
            r = 90;
        else
            r = -90;
        highlight.transform.rotation = Quaternion.Euler(0, 0, r);
    }

    public void RotationAnim(Rotation r)
    {
        Vector2Int dir = parent.currentDir;
        SetArrowDir(dir);

        highlight.transform.DORotate(new Vector3(0, 0, highlight.transform.rotation.eulerAngles.z + (r.ToFloat() * Mathf.Rad2Deg)), 1).SetEase(Ease.OutBack).SetLink(gameObject);
        if (currentFadeTween != null && currentFadeTween.IsActive())
            currentFadeTween.Kill();
        currentFadeTween = highlight.DOFade(0, 2).From(1).SetLink(gameObject);
    }

    public IEnumerator ApplyFieldRotate()
    {
        /*
        fieldType
        0: Horizontal
        1: Vertical
        2: left to botton
        3: left to top
        4: right to top
        5: right to bottom
        6: end
         */

        if (fieldType == 0)
        {
            yield break;
        }
        else if (fieldType == 1)
        {
            yield break;
        }
        else if (fieldType == 2)
        {
            if (parent.currentDir == Vector2Int.right)
            {
                yield return RotationProcess(Rotation.m90);
            }
            else if (parent.currentDir == Vector2Int.up)
            {
                yield return RotationProcess(Rotation.p90);
            }
        }
        else if (fieldType == 3)
        {
            if (parent.currentDir == Vector2Int.right)
            {
                yield return RotationProcess(Rotation.p90);
            }
            else if (parent.currentDir == Vector2Int.down)
            {
                yield return RotationProcess(Rotation.m90);
            }
        }
        else if (fieldType == 4)
        {
            if (parent.currentDir == Vector2Int.left)
            {
                yield return RotationProcess(Rotation.m90);
            }
            else if (parent.currentDir == Vector2Int.down)
            {
                yield return RotationProcess(Rotation.p90);
            }
        }
        else if (fieldType == 5)
        {
            if (parent.currentDir == Vector2Int.left)
            {
                yield return RotationProcess(Rotation.p90);
            }
            else if (parent.currentDir == Vector2Int.up)
            {
                yield return RotationProcess(Rotation.m90);
            }
        }
        else if (fieldType == 6)
        {
            if (parent.currentDir == Vector2Int.right)
            {
                parent.currentPos = new Vector2Int(-1, -1);
            }
        }


        yield break;
    }

    IEnumerator RotationProcess(Rotation r)
    {
        var act = new Rotate("ROT", r);

        RotationAnim(r);
        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(systemInstance);
    }

    public void OnDeselect()
    {

    }

    public void OnSelect()
    {

    }

    //-- Game Logic --//
    public IEnumerator Activate(Vector2Int dir)
    {
        ActivationAnim(dir);

        if (cardInstance == null)
            yield break;

        yield return cardInstance.Activate(dir);
    }

    public IEnumerator CardInitialize()
    {
        if (cardInstance == null)
            yield break;

        yield return cardInstance.Initialize();
    }

    public IEnumerator CardStart()
    {
        if (cardInstance == null)
            yield break;

        yield return cardInstance.Start();
    }
}
