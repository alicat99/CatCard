using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    throw new System.Exception("GameManger doesn`t exist");
                }
                instance.Initiate();
            }
            return instance;
        }
    }
    private static GameManager instance;

    private bool isInitiated;

    public CardManager card;
    public FloatingTextManager floatingText;
    public UIEffectBubbleManager uiEffectBubble;

    private void Awake()
    {
        instance = this;

        Initiate();
    }

    private void Initiate()
    {
        if (isInitiated)
            return;

        card = GetComponent<CardManager>();
        card.Initialize();
        floatingText = GetComponent<FloatingTextManager>();
        floatingText.Initialize();
        uiEffectBubble = GetComponent<UIEffectBubbleManager>();
        uiEffectBubble.Initialize();
    }
}
