using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Rotate: CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Act act = new Rotate("ROT", Rotation.m90);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "rotation", 0);
        act.AddEffect(effect);

        var currentFieldItem = instance.field.GetItem(instance.pos);
        currentFieldItem.RotationAnim(Rotation.m90);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);


        act = new Del("DEL", instance);

        yield return act.Invoke(instance);
    }
}
