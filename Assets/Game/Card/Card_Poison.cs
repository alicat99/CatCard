using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_Poison : CardData
{
    public override IEnumerator OnStart(Entity[] es, int slotCount, int intensity)
    {
        Query query = new Query();

        if (intensity > 0)
        {
            var act = new Remain(slotCount);
            query.Add(act);
            var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Magic", 1);
            query.AddBubble(act, effect);

            var i = new SetIntensity(slotCount, intensity - 1);
            query.Add(i);
        }
        else if (intensity == 0)
        {
            var act = new Attack(es[1], 2);
            query.Add(act);
            var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Attack", 2);
            query.AddBubble(act, effect);
            var del = new Del(slotCount);
            query.Add(del);
        }

        yield return new WaitForSeconds(0.5f);
        yield return query.Process($"{es[0].entityType}{slotCount:00}Poison");
    }

    public override IEnumerator OnInitialize(Entity[] es, int slotCount, int intensity)
    {
        var act = new AddTrigger(new Trigger("A", "A", query => Trigger(query, slotCount, es), query =>
        {
            foreach (var act in query)
            {
                if (!act.GetType().IsAssignableFrom(typeof(Del)))
                    continue;
                var del = (Del)act;
                Debug.Log($"{CardField.Slot2Count(del.pos)} {slotCount}");
                if (CardField.Slot2Count(del.pos) == slotCount)
                    return true;
            }
            return false;
        }));
        Query query = new Query(act);

        yield return query.Process($"{es[0].entityType}{slotCount:00}AddTrigger");
    }

    private IEnumerator Trigger(Query query, int slotCount, Entity[] es)
    {
        bool isUseful = false;
        int decrease = 0;

        foreach (var a in query)
        {
            if (a.GetType().IsAssignableFrom(typeof(Attack)))
            {
                Attack attack = (Attack)a;
                if (attack.target != es[0])
                    continue;

                isUseful = true;

                decrease += 1;
            }
        }

        if (isUseful)
        {
            var slot = GameManager.Instance.card.field.GetSlot(slotCount);
            int newIntensity = slot.intensity - decrease;
            if (newIntensity < 0)
            {
                var del = new Del(slotCount);
                query.Add(del);
            }
            else
            {
                var act = new SetIntensity(slotCount, newIntensity);
                query.Add(act);

                var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Magic", decrease);
                query.AddBubble(act, effect);
            }
            yield return new WaitForSeconds(0.5f);
        }
        else
            yield break;
    }
}
