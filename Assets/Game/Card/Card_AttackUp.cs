using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_AttackUp : CardData
{
    public override IEnumerator OnActivate(Entity[] es, int slotCount, Vector2Int dir, int intensity)
    {
        int currentCardCount = GameManager.Instance.card.field.cardCount;
        var act = new AddTrigger(new Trigger("P", "_", query =>
        {
            if (GameManager.Instance.card.field.cardCount == currentCardCount + 1)
                return Trigger(query);
            return null;
        }, query =>
        {
            var trig = query.lastTrigger;
            if (CardSystem.TestTrigger(trig, "AS00TurnEnd"))
                return true;

            if (!CardSystem.TestTrigger(trig, "P"))
                return false;

            if (CardSystem.TestTrigger(trig, "_S"))
                return false;

            return true;
        }));
        Query query = new Query(act);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Magic", "magic", 0);
        query.AddBubble(act, effect);

        yield return query.Process($"{es[0].entityType}{slotCount:00}AddTrigger");

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Trigger(Query query)
    {
        bool isUseful = false;

        foreach (var a in query)
        {
            if (a.GetType().IsAssignableFrom(typeof(Attack)))
            {
                isUseful = true;

                Attack attack = (Attack)a;
                attack.damage += 2;

                if (query.TryGetBubble(a, out var bubble))
                {
                    bubble.SetIntensity(attack.damage);
                    bubble.AddDuration(0.5f);
                }
            }
        }

        if (isUseful)
            yield return new WaitForSeconds(0.5f);
        else
            yield break;
    }
}
