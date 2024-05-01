using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Shield : CardData
{
    public override IEnumerator OnInitialize(CardInstance instance)
    {
        var trigger = new Trigger(
            "B/ATK", act => ((AddHealth)act).target.entityType == instance.entityType,
            "A/DEL", act => ((Del)act).target == instance,
            act => OnTrigger(instance, act as AddHealth));

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

    private IEnumerator OnTrigger(CardInstance instance, AddHealth act)
    {
        if (instance.intensity >= -act.amount)
        {
            act.cancel = true;
            if (act.TryGetEffect(out var effect))
            {
                effect.Cancel();
                effect.AddDuration();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            int newAmount = act.amount + instance.intensity;
            if (newAmount >= 0) newAmount = 0;

            act.amount = newAmount;

            if (act.TryGetEffect(out var effect))
            {
                effect.AddDuration();
                effect.SetIntensity(Mathf.Abs(newAmount));
            }

            yield return new WaitForSeconds(0.5f);
        }

        int newIntensity = instance.intensity + act.amount;

        if (newIntensity <= 0)
        {
            Act act2 = new Del("DEL", instance);

            yield return act2.Invoke(instance);

            yield break;
        }
        else
        {
            Act act2 = new SetIntensity("INT", instance, newIntensity);

            yield return act2.Invoke(instance);

            yield break;
        }
    }
}
