using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Bleeding : CardData
{
    public override IEnumerator OnStart(CardInstance instance)
    {
        Entity target = instance.field.GetEntity(instance.entityType.Inverse());
        Act act = new AddHealth("ATK", -1, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", 1);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);

        if (instance.intensity > 0)
        {
            act = new Remain("REM", instance, true);

            effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "preservation", 0);
            act.AddEffect(effect);

            yield return new WaitForSeconds(0.5f);

            yield return act.Invoke(instance);

            act = new SetIntensity("INT", instance, instance.intensity - 1);

            yield return act.Invoke(instance);
        }
    }

    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Entity target = instance.field.GetEntity(instance.entityType);
        Act act = new AddHealth("ATK", -1, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", 1);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }
}
