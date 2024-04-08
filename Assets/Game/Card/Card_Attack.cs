using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Act;

public class Card_Attack : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Entity target = instance.field.GetEntity(instance.entityType.Inverse());
        Act.Act act = new AddHealth("ATK", -instance.intensity, target);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", instance.intensity);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);
    }
}
