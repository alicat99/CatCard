using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ActSystem;

public class CardField : MonoBehaviour
{
    [SerializeField]
    GameObject cardFieldItemPrefab;
    public CardSelector cardSelector;
    public CardUIBackground background;
    public BattlegroundUI battleground;

    private CardFieldItem[,] field;

    [HideInInspector]
    public Vector2Int currentDir;
    [HideInInspector]
    public Vector2Int currentPos;
    public CardFieldItem currentSlot { get => GetItem(currentPos); }
    public bool isRunning { get; private set; }
    public int slotCounter { get; private set; }
    public int cardCounter { get; private set; }

    const int CARD_INVOKE_COUNT = 10;

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
                cardFieldItem.Initialize(this, types[i, j], new Vector2Int(i, j));
            }
        }
        RandomFill();

        slotCounter = 0;
        cardCounter = 0;

        CardSystem.Reset();
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
                if (item.cardInstance != null)
                {
                    var instance = item.cardInstance;
                    if (!instance.isInitialized)
                    {
                        CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
                        yield return item.CardInitialize();
                        CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
                        yield return CardSystem.InvokeTrigger(null, "A/INI");
                    }

                    CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
                    yield return item.CardStart();
                }
            }
        }

        int i;
        var w = new WaitForSeconds(0.05f);
        for (i = 0; i < 50; i++)
        {
            if (NotInField())
                break;

            ++slotCounter;
            var s = currentSlot;
            CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
            if (s.cardInstance != null)
                ++cardCounter;
            yield return s.Activate(currentDir);
            if (NotInField())
                break;

            yield return currentSlot.ApplyFieldRotate();
            if (NotInField())
                break;

            yield return w;

            currentPos = AddDirToPos(currentPos, currentDir);
        }
        if (i == 25)
        {
            Alert(Utils.GetLocalizedString("card_excution_reached_maximum"));
            yield return new WaitForSeconds(0.5f);
        }
        Alert(Utils.GetLocalizedString("turn_end"));

        CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
        yield return CardSystem.InvokeTrigger(null, "A/END");
        FloatingTextManager.Print(Utils.GetLocalizedString("turn_end"), new Vector2(0, 500), Color.white);

        w = new WaitForSeconds(0.1f);
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                CardFieldItem item = field[x, y];
                var instance = item.cardInstance;
                if (instance == null)
                    continue;
                if (instance.isRemaining)
                {
                    instance.isRemaining = false;
                    continue;
                }
                var act = new Del("DEL/SYS", instance);
                CardSystem.ResetTriggerCount(CARD_INVOKE_COUNT);
                yield return act.Invoke(item.systemInstance);
                yield return w;
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

    public CardFieldItem GetItem(Vector2Int pos)
    {
        if (NotInField(pos))
            return null;
        return field[pos[0], pos[1]];
    }

    public Entity GetEntity(EntityType type)
    {
        return type == EntityType.P ? battleground.es[0] : battleground.es[1];
    }

    public void Alert(string content)
    {
        FloatingTextManager.Print(content, new Vector2(0, 500), Color.white);
    }

    public static Vector2Int AddDirToPos(Vector2Int pos, Vector2Int dir)
    {
        return new Vector2Int(pos.x - dir.y, pos.y + dir.x);
    }

    //temp
    private void RandomFill()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (field[i, j].cardInstance == null && Random.value < 0.3f)
                {
                    SetCard(i, j, GameManager.Instance.card.GetRandomCard(), EntityType.E);
                }
            }
        }
    }
}
