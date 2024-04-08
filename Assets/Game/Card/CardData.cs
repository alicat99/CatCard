using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public Sprite icon;
    public string cardName;
    public string description;
    public int intensity;

    public virtual IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        yield break;
    }

    public virtual IEnumerator OnInitialize(CardInstance instance)
    {
        yield break;
    }

    public virtual IEnumerator OnStart(CardInstance instance)
    {
        yield break;
    }
}
