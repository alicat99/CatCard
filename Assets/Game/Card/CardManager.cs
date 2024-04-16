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
    [SerializeField]
    ScriptableObjectList templates;

    //temp
    public CardData GetRandomCard()
    {
        return cardDatas[Random.Range(0, cardDatas.Length)];
    }

    //temp
    public ICardTemplate GetRandomTempate()
    {
        int totalWeight = 0;
        foreach (ICardTemplate item in templates.data)
        {
            totalWeight += item.GetWeight();
        }

        // 랜덤 가중치 생성
        double randomWeight = Random.Range(0, totalWeight);

        // 누적 가중치 합계를 사용하여 선택
        double cumulativeWeight = 0;
        foreach (ICardTemplate item in templates.data)
        {
            cumulativeWeight += item.GetWeight();
            if (randomWeight < cumulativeWeight)
            {
                return item;
            }
        }
        return templates.data[0] as ICardTemplate;
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
