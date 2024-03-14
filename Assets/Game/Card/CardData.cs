using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public Sprite icon;
    public string cardName;
    [Multiline]
    public string description;
    public int intensity;

    public virtual IEnumerator OnActivate(Entity[] es, int slotCount, Vector2Int dir, int intensity)
    {
        yield break;
    }

    public virtual IEnumerator OnInitialize(Entity[] es, int slotCount, int intensity)
    {
        yield break;
    }

    public virtual IEnumerator OnStart(Entity[] es, int slotCount, int intensity)
    {
        yield break;
    }
}
