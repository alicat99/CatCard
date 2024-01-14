using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSelector : MonoBehaviour
{
    [SerializeField]
    GameObject cardSelectorItemPrefab;
    [SerializeField]
    Transform content;
    public CardUIBackground background;
    [SerializeField]
    Button checkButton;

    ObjectPoolManager pool;

    [HideInInspector]
    public CardSelectorItem selectedItem;

    private void Start()
    {
        pool = ObjectPoolManager.Instance;
        pool.InitializeSpawn(cardSelectorItemPrefab, 20);

        AddCard(0, GameManager.Instance.card.GetRandomCard());
        AddCard(0, GameManager.Instance.card.GetRandomCard());
        AddCard(0, GameManager.Instance.card.GetRandomCard());
        AddCard(0, GameManager.Instance.card.GetRandomCard());
    }

    public CardData PopCard(CardSelectorItem cardSelectorItem)
    {
        pool.Despawn(cardSelectorItem.gameObject);
        return cardSelectorItem.cardData;
    }

    public bool AddCard(int index, CardData cardData)
    {
        if (pool.GetActiveCount(cardSelectorItemPrefab.name) == 20)
        {
            return false;
        }
        GameObject obj = pool.Spawn(cardSelectorItemPrefab.name, content);
        obj.transform.localScale = Vector3.one;
        CardSelectorItem item = obj.GetComponent<CardSelectorItem>();
        item.SetData(cardData, this);
        obj.transform.SetSiblingIndex(index);
        return true;
    }

    private void Update()
    {
        //temp
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard(0, GameManager.Instance.card.GetRandomCard());
        }
    }

    private void OnDestroy()
    {
        ObjectPoolManager.Instance?.DespawnPool(cardSelectorItemPrefab.name);
    }
}
