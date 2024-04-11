using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Flip : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Act act = new Rotate("ROT", Rotation.p180);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "rotation", 0);
        act.AddEffect(effect);

        var currentFieldItem = instance.field.GetItem(instance.pos);
        currentFieldItem.RotationAnim(Rotation.p180);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);


        Entity target = instance.field.GetEntity(instance.entityType);
        int amount = instance.intensity;
        act = new AddHealth("ATK", -amount, target);

        effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Attack", "attack", amount);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);


        act = new Del("DEL", instance);

        yield return act.Invoke(instance);
    }
}
