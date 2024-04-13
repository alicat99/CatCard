using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSystem;

public class Card_Purification : CardData
{
    public override IEnumerator OnActivate(CardInstance instance, Vector2Int dir)
    {
        bool useEffect = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var target = instance.field.GetItem(new Vector2Int(i, j));

                if (target == null || target.cardInstance == null)
                    continue;

                int newIntensity = target.cardInstance.intensity;
                if (newIntensity <= 0)
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
        }
        yield break;
    }
}
