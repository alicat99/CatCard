using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class CardManager : MonoBehaviour
{
    public CardSelector selector;
    public CardField field;
    public BattlegroundUI battleground;
    public CardUIBackground background;

    public CardData dummyCardData;

    //temp
    [Multiline(15)]
    public string displayTrigger;

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

    //temp
    public void Update()
    {
        displayTrigger = "";
        foreach (var trg in CardSystem.triggers)
        {
            displayTrigger += trg.required + "\n";
        }
    }
}
