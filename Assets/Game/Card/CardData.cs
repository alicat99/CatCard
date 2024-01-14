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

    public virtual IEnumerator OnActivate(Entity[] es, int slot, Vector2Int dir, int intensity)
    {
        yield break;
    }
}
