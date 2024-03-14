using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Act;

public class CardField : MonoBehaviour
{
    [SerializeField]
    GameObject cardFieldItemPrefab;
    public CardSelector cardSelector;
    public CardUIBackground background;
    public BattlegroundUI battleground;

    public CardFieldItem[,] field;

    [HideInInspector]
    public Vector2Int currentDir;
    [HideInInspector]
    public Vector2Int currentPos;
    public int slotCount
    {
        get
        {
            return Slot2Count(currentPos);
        }
    }
    public int cardCount { get; private set; }
    public CardFieldItem currentSlot { get => GetSlot(currentPos); }
    public bool isRunning { get; private set; }

    private void Start()
    {
        field = new CardFieldItem[3, 3];
        int[,] types = new int[,] { { 0, 0, 2 }, { 5, 6, 1 }, { 4, 0, 3 } };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var obj = Instantiate(cardFieldItemPrefab, transform);
                CardFieldItem cardFieldItem = obj.GetComponent<CardFieldItem>();
                field[i, j] = cardFieldItem;
                cardFieldItem.Initialize(this, types[i, j]);
            }
        }
        RandomFill();
    }

    public void SetCard(int i, int j, CardData cardData, EntityType type)
    {
        field[i, j].SetData(cardData, type);
    }

    public IEnumerator RunField()
    {
        isRunning = true;

        currentDir = Vector2Int.right;
        currentPos = Vector2Int.zero;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                CardFieldItem item = field[x, y];
                if (item.cardData != null)
                {
                    int count = Slot2Count(new Vector2Int(x, y));

                    if (!item.isCardInitialized)
                    {
                        item.isCardInitialized = true;

                        yield return item.InitializeCard(count);
                        yield return CardSystem.InvokeTrigger(new Query(), $"AS{count:00}Init");
                    }

                    yield return item.StartCard(count);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);

        int i;
        var w = new WaitForSeconds(0.05f);
        cardCount = 0;
        for (i = 0; i < 50; i++)
        {
            if (NotInField())
                break;

            var s = currentSlot;
            var lastDir = currentDir;
            yield return s.Activate(lastDir);
            if (NotInField())
                break;

            yield return currentSlot.ApplyFieldRotate();
            if (NotInField())
                break;

            yield return w;

            currentPos += new Vector2Int(-currentDir[1], currentDir[0]);
            cardCount += 1;
        }
        if (i == 25)
        {
            FloatingTextManager.Print(Utils.GetLocalizedString("card_excution_reached_maximum"), new Vector2(0, 500), Color.white);
            yield return new WaitForSeconds(0.5f);
        }


        yield return CardSystem.InvokeTrigger(new Query(), "AS00TurnEnd");
        FloatingTextManager.Print(Utils.GetLocalizedString("turn_end"), new Vector2(0, 500), Color.white);

        w = new WaitForSeconds(0.1f);
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                CardFieldItem item = field[x, y];
                if (item.isRemaining)
                {
                    item.isRemaining = false;
                    continue;
                }
                if (item.cardData != null)
                {
                    var query = new Query(new Del(new Vector2Int(x, y)));
                    yield return query.Process($"S{x * 3 + y:00}Del");
                    yield return w;
                }
            }
        }
        yield return new WaitForSeconds(0.3f);

        RandomFill();

        cardSelector.AddCard(0, GameManager.Instance.card.GetRandomCard());
        cardSelector.AddCard(0, GameManager.Instance.card.GetRandomCard());

        isRunning = false;

        yield break;
    }

    private bool NotInField()
    {
        return !(0 <= currentPos[0] && currentPos[0] < 3 && 0 <= currentPos[1] && currentPos[1] < 3);
    }

    private bool NotInField(Vector2Int pos)
    {
        return !(0 <= pos[0] && pos[0] < 3 && 0 <= pos[1] && pos[1] < 3);
    }

    public static int Slot2Count(Vector2Int pos)
    {
        return pos[1] + (pos[0] * 3);
    }

    public static Vector2Int Count2Slot(int count)
    {
        return new Vector2Int(count / 3, count % 3);
    }

    public CardFieldItem GetSlot(Vector2Int pos)
    {
        if (NotInField(pos))
            return null;
        return field[pos[0], pos[1]];
    }

    public CardFieldItem GetSlot(int count)
    {
        return GetSlot(Count2Slot(count));
    }

    //temp
    private void RandomFill()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (field[i, j].cardData == null && Random.value < 0.5f)
                {
                    SetCard(i, j, GameManager.Instance.card.GetRandomCard(), EntityType.E);
                }
            }
        }
    }
}
