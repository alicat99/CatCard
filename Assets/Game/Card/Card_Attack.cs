using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_Attack : CardData
{
    public override IEnumerator OnActivate(Entity[] es, int slotCount, Vector2Int dir, int intensity)
    {
        var act = new Attack(es[1], intensity);
        Query query = new Query(act);
        
        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Damage", intensity);
        query.AddBubble(act, effect);

        yield return new WaitForSeconds(0.5f);
        yield return query.Process($"{es[0].entityType}{slotCount:00}Attack");
    }
}
