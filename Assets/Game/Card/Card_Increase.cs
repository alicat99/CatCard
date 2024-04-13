using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Increase : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        bool useEffect = false;

        for (int i = 0; i < 2; i++)
        {
            int y = i * 2 - 1;
            Vector2Int nextPos = CardField.AddDirToPos(instance.pos, new Vector2Int(0, y));
            var target = instance.field.GetItem(nextPos);

            if (target == null || target.cardInstance == null)
                continue;

            int newIntensity = target.cardInstance.intensity;
            if (newIntensity == 0)
                continue;
            newIntensity += instance.intensity;
            Act act = new SetIntensity("INT", target.cardInstance, newIntensity);

            if (!useEffect)
            {
                useEffect = true;

                var effect = GameManager.Instance.uiEffectBubble.PrintBySlot(instance.pos, "Magic", "magic", instance.intensity);
                act.AddEffect(effect);

                yield return new WaitForSeconds(0.5f);
            }

            yield return act.Invoke(instance);

            target.UpdateUI();
        }
        yield break;
    }
}
