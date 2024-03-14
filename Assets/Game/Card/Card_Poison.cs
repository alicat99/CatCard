using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_Poison : CardData
{
    public override IEnumerator OnStart(Entity[] es, int slotCount, int intensity)
    {
        if (intensity > 0)
        {
            Query query = new Query();

            var act = new Remain(slotCount);
            query.Add(act);
            var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Magic", "preservation", 0);
            query.AddBubble(act, effect);

            var set = new SetIntensity(slotCount, intensity - 1);
            query.Add(set);

            yield return new WaitForSeconds(0.5f);
            yield return query.Process($"{es[0].entityType}{slotCount:00}Remain");
        }
        else
        {
            yield break;
        }
    }

    public override IEnumerator OnActivate(Entity[] es, int slotCount, Vector2Int dir, int intensity)
    {
        var act = new Attack(es[1], 1);
        Query query = new Query(act);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Attack", "attack", act.damage);
        query.AddBubble(act, effect);

        yield return new WaitForSeconds(0.5f);
        yield return query.Process($"{es[0].entityType}{slotCount:00}Attack");
    }

    public override IEnumerator OnInitialize(Entity[] es, int slotCount, int intensity)
    {
        var act = new AddTrigger(new Trigger("A", "A", query => Trigger(query, slotCount, es), query => Utils.IsDeleted(query, slotCount)));
        Query query = new Query(act);

        yield return query.Process($"{es[0].entityType}{slotCount:00}AddTrigger");
    }


    private IEnumerator Trigger(Query query, int slotCount, Entity[] es)
    {
        if (!Utils.IsDeleted(query, slotCount))
            yield break;

        var act = new Attack(es[0], 1);
        query.Add(act);
        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(slotCount, "Attack", "attack", act.damage);
        query.AddBubble(act, effect);

        yield return new WaitForSeconds(0.5f);
    }
}
