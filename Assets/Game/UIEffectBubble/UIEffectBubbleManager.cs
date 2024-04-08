using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectBubbleManager : MonoBehaviour
{
    [SerializeField]
    GameObject effectPrefab;
    [SerializeField]
    ScriptableObjectList iconList;

    ObjectPoolManager objPool;
    CardManager card;

    Dictionary<string, Sprite> iconDict;

    Canvas canvas
    {
        get
        {
            if (_canvas == null)
            {
                var cs = FindObjectsOfType<Canvas>();
                foreach (var c in cs)
                {
                    if (_canvas == null || _canvas.sortingOrder < c.sortingOrder)
                    {
                        _canvas = c;
                    }
                }
            }

            if (_canvas == null)
                Debug.LogError("To use floatingtext, please create canvas on scene");
            return _canvas;
        }
    }
    Canvas _canvas;

    public void Initialize()
    {
        objPool = ObjectPoolManager.Instance;
        objPool.InitializeSpawn(effectPrefab, 10, 10, AP_enum.EmptyBehavior.ReuseOldest, AP_enum.MaxEmptyBehavior.ReuseOldest);

        card = GameManager.Instance.card;

        iconDict = new Dictionary<string, Sprite>(iconList.data.Length);
        foreach (UIEffectBubbleIconSO so in iconList.data)
        {
            iconDict.Add(so.name, so.sprite);
        }
    }

    public UIEffectBubble Print(Vector2 position, string iconName, string description, int intensity = 0)
    {
        var obj = objPool.Spawn(effectPrefab.name);
        Sprite sprite;
        if (!iconDict.TryGetValue(iconName, out sprite))
        {
            sprite = null;
        }

        UIEffectBubble uIEffectBubble = obj.GetComponent<UIEffectBubble>();
        uIEffectBubble.Initialize(position, canvas.transform, sprite, intensity, description);

        return uIEffectBubble;
    }

    public UIEffectBubble PrintBySlot(Vector2Int slot, string iconName, string description, int intensity = 0)
    {
        Vector2 position = card.field.GetItem(slot).transform.position;
        //Vector2 position = card.field.GetSlot(slot).GetComponent<RectTransform>().position;
        return Print(position, iconName, description, intensity);
    }

    private void OnDestroy()
    {
        ObjectPoolManager.Instance?.DespawnPool(effectPrefab.name);
    }
}
