using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_Rotate : CardData
{
    public override IEnumerator OnActivate(Entity[] es, int slotCount, Vector2Int dir, int intensity)
    {

        var act = new RotateDir(Rotation.m90);
        Query query = new Query(act);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Magic", 0);
        query.AddBubble(act, effect);

        yield return new WaitForSeconds(0.5f);
        yield return query.Process($"{es[0].entityType}{slotCount:00}Rotate");

        query = new Query(new Del(slotCount));
        yield return query.Process($"{es[0].entityType}{slotCount:00}Del");
    }
}
