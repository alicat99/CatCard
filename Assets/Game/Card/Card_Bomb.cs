using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Bomb : CardData
{
    public override IEnumerator OnStart(CardInstance instance)
    {
        var identity = instance.field.GetEntity(instance.entityType);
        var trigger = new Trigger(
            "A/ATK", act => ((AddHealth)act).target == identity,
            "A/END", act => true,
            act => OnTrigger(instance))
            .SetLink(instance);

        Act act = new AddTrigger("TRG", trigger);

        yield return act.Invoke(instance);
    }

    private IEnumerator OnTrigger(CardInstance instance)
    {
        Vector2Int pos = instance.pos;
        var field = instance.field;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) || (i != 0 && j != 0))
                    continue;

                var d = new Vector2Int(i, j);
                var newPos = CardField.AddDirToPos(pos, d);
                var target = field.GetItem(newPos);

                if (target == null || target.cardInstance == null)
                    continue;

                Act act = new Del("DEL", target.cardInstance);

                var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "magic", 0);
                act.AddEffect(effect);

                yield return new WaitForSeconds(0.5f);

                yield return act.Invoke(instance);
            }
        }

        yield break;
    }
}
