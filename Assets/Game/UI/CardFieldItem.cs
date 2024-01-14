using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Act;

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

    public CardData cardData { get; private set; }
    public CardField parent { get; private set; }
    public EntityType type { get; private set; }
    public int fieldType { get; private set; }
    public int intensity
    {
        get => _intensity;
        set
        {
            _intensity = value;
            if (_intensity == 0)
            {
                intensityText.enabled = false;
            }
            else
            {
                intensityText.enabled = true;
                intensityText.text = _intensity.ToString();
            }
        }
    }
    private int _intensity;

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

    public bool isCardInitialized = false;

    private Tween currentFadeTween = null;

    public void Initialize(CardField parent, int fieldType)
    {
        this.parent = parent;
        GetComponent<Button>().onClick.AddListener(OnClick);
        GetComponent<Image>().sprite = sprites[fieldType];
        this.fieldType = fieldType;
    }

    private void OnClick()
    {
        if (cardData == null)
        {
            CardSelector cardSelector = parent.cardSelector;
            if (cardSelector.selectedItem == null)
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
            parent.background.SelectData(this, cardData, intensity);
        }
    }

    public void SetData(CardData cardData, EntityType type)
    {
        this.cardData = cardData;
        this.type = type;

        if (cardData == null)
        {
            visibility = false;
            intensityText.enabled = false;
            return;
        }

        visibility = true;
        intensity = cardData.intensity;
        icon.sprite = cardData.icon;
        var c = type == EntityType.P ? Color.white : Color.red;
        c.a = icon.color.a;
        icon.color = c;
        isCardInitialized = false;
    }

    public IEnumerator Activate(Vector2Int dir)
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

        currentFadeTween = highlight.DOFade(0, 1).From(1).SetLink(gameObject);
        var battleground = parent.battleground;
        if (cardData == null)
            yield break;
        Entity[] es = battleground.es;
        if (type == EntityType.E)
            es = new Entity[] { es[1], es[0] };

        yield return cardData.OnActivate(es, parent.slotCount, dir, intensity);
    }

    public IEnumerator RotateAnim(Rotation r)
    {
        highlight.transform.DORotate(new Vector3(0, 0, highlight.transform.rotation.eulerAngles.z + (r.ToFloat() * Mathf.Rad2Deg)), 1).SetEase(Ease.OutBack).SetLink(gameObject);
        if (currentFadeTween != null && currentFadeTween.IsActive())
            currentFadeTween.Kill();
        currentFadeTween = highlight.DOFade(0, 2).From(1).SetLink(gameObject);
        yield return new WaitForSeconds(0.5f);
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
                var query = new Query(new RotateDir(Rotation.m90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
            else if (parent.currentDir == Vector2Int.up)
            {
                var query = new Query(new RotateDir(Rotation.p90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
        }
        else if (fieldType == 3)
        {
            if (parent.currentDir == Vector2Int.right)
            {
                var query = new Query(new RotateDir(Rotation.p90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
            else if (parent.currentDir == Vector2Int.down)
            {
                var query = new Query(new RotateDir(Rotation.m90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
        }
        else if (fieldType == 4)
        {
            if (parent.currentDir == Vector2Int.left)
            {
                var query = new Query(new RotateDir(Rotation.m90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
            else if (parent.currentDir == Vector2Int.down)
            {
                var query = new Query(new RotateDir(Rotation.p90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
        }
        else if (fieldType == 5)
        {
            if (parent.currentDir == Vector2Int.left)
            {
                var query = new Query(new RotateDir(Rotation.p90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
            }
            else if (parent.currentDir == Vector2Int.up)
            {
                var query = new Query(new RotateDir(Rotation.m90));
                int slotCount = parent.slotCount;
                yield return query.Process($"S{slotCount:00}Rotate");
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

    public void OnDeselect()
    {

    }

    public void OnSelect()
    {

    }
}
