using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_AttackUp : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        var field = instance.field;
        int currentSlot = instance.field.slotCounter;

        var trigger = new Trigger(
            "B/ATK", act => field.slotCounter == currentSlot + 1,
            "_", act => field.slotCounter > currentSlot,
            act => OnTrigger(act));

        Act act = new AddTrigger("TRG", trigger);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "magic", 0);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }

    private IEnumerator OnTrigger(Act act)
    {
        AddHealth addHealth = (AddHealth)act;
        int a = addHealth.amount;
        a = a > 0 ? a + 1 : a - 1;
        addHealth.amount = a;

        if (act.TryGetEffect(out var effect))
        {
            effect.SetIntensity(Mathf.Abs(a));
            effect.AddDuration();
        }

        yield return new WaitForSeconds(0.5f);
    }
}
