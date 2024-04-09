using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Regen : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Entity target = instance.field.GetEntity(instance.entityType);
        Act act = new AddHealth("REG", instance.intensity, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Regen", "regen", instance.intensity);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }
}
