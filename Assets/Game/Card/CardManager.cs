using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardSelector selector;
    public CardField field;
    public BattlegroundUI battleground;
    public CardUIBackground background;

    //temp
    [SerializeField]
    CardData[] cardDatas;

    //temp
    public CardData GetRandomCard()
    {
        return cardDatas[Random.Range(0, cardDatas.Length)];
    }

    public void Initialize()
    {

    }
}
