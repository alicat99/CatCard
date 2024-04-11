using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Increase : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        Vector2Int nextPos = CardField.AddDirToPos(instance.pos, dir);
        var target = instance.field.GetItem(nextPos);
        
        if (target == null || target.cardInstance == null)
            yield break;

        int newIntensity = target.cardInstance.intensity;
        if (newIntensity == 0)
            yield break;
        newIntensity += instance.intensity;
        Act act = new SetIntensity("INT", target.cardInstance, newIntensity);

        var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "magic", instance.intensity);
        act.AddEffect(effect);

        yield return new WaitForSeconds(0.5f);

        yield return act.Invoke(instance);

        target.UpdateUI();
    }
}
