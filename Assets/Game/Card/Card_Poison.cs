using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Poison : CardData
{
    public override IEnumerator OnInitialize(CardInstance instance)
    {
        var trigger = new Trigger(
            instance,
            "A/DEL", act => ((Del)act).target == instance,
            "A/DEL", act => ((Del)act).target == instance,
            act => OnTrigger(instance));

        Act act = new AddTrigger("TRG", trigger);

        yield return act.Invoke(instance);
    }

    public override IEnumerator OnStart(CardInstance instance)
    {
        if (instance.intensity > 0)
        {
            Act act = new Remain("REM", instance, true);

            var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "preservation", 0);
            act.AddEffect(effect);

            yield return new WaitForSeconds(0.5f);

            yield return act.Invoke(instance);

            act = new SetIntensity("INT", instance, instance.intensity - 1);

            yield return act.Invoke(instance);
        }
    }

    private IEnumerator OnTrigger(CardInstance instance)
    {
        Entity target = instance.field.GetEntity(instance.entityType);
        var act = new AddHealth("ATK", -2, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", -act.amount);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }

    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Entity target = instance.field.GetEntity(instance.entityType.Inverse());
        var act = new AddHealth("ATK", -1, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", -act.amount);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }
}
