using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance
{
    public readonly CardData card;
    public readonly CardField field;
    public Vector2Int pos;
    public int intensity;
    public EntityType entityType;
    public bool isInitialized;
    public bool isRemaining;
    public bool isAlive;

    public CardInstance(Vector2Int pos, CardData cardData, EntityType entityType, CardField field)
    {
        this.pos = pos;
        card = cardData;
        intensity = cardData.intensity;
        this.entityType = entityType;
        this.field = field;
        isInitialized = false;
        isRemaining = false;
        isAlive = true;
    }

    public IEnumerator Activate(Vector2Int dir)
    {
        yield return card.OnActivate(this, dir);
    }

    public IEnumerator Initialize()
    {
        isInitialized = true;
        yield return card.OnInitialize(this);
    }

    public IEnumerator Start()
    {
        yield return card.OnStart(this);
    }
}
