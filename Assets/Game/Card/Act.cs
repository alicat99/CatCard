using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Act
{
    public interface IAct
    {
        public IEnumerator Activate();
    }

    public class Attack: IAct
    {
        public Entity target;
        public int damage;

        public Attack(Entity target, int damage)
        {
            this.target = target;
            this.damage = damage;
        }

        public IEnumerator Activate()
        {
            target.health -= damage;
            yield break;
        }
    }

    public class Regen : IAct
    {
        public Entity target;
        public int heal;

        public Regen(Entity target, int heal)
        {
            this.target = target;
            this.heal = heal;
        }

        public IEnumerator Activate()
        {
            target.health += heal;
            yield break;
        }
    }

    public class AddTrigger : IAct
    {
        public Trigger trigger;

        public AddTrigger(Trigger trigger)
        {
            this.trigger = trigger;
        }

        public IEnumerator Activate()
        {
            CardSystem.InitializeTrigger(trigger);
            yield break;
        }
    }

    public class RotateDir: IAct
    {
        public Rotation r;

        public RotateDir(Rotation r)
        {
            this.r = r;
        }

        public IEnumerator Activate()
        {
            CardField field = GameManager.Instance.card.field;
            field.currentDir = field.currentDir.Rotate(r);
            yield return field.currentSlot.RotateAnim(r);
        }
    }

    public class Del: IAct
    {
        public Vector2Int pos;

        public Del(Vector2Int pos)
        {
            this.pos = pos;
        }

        public Del(int slotCount)
        {
            pos = new Vector2Int(slotCount / 3, slotCount % 3);
        }

        public IEnumerator Activate()
        {
            CardField field = GameManager.Instance.card.field;
            field.SetCard(pos.x, pos.y, null, EntityType.P);
            yield break;
        }
    }

    public class Trigger
    {
        public string required { get; private set; }
        public string end { get; private set; }
        Func<Query, IEnumerator> onTrigger;
        Func<Query, bool> onEnd;

        public Trigger(string required, string end, Func<Query, IEnumerator> onTrigger, Func<Query, bool> onEnd = null)
        {
            this.required = required;
            this.end = end;
            this.onTrigger = onTrigger;
            this.onEnd = onEnd;
        }

        public IEnumerator OnTrigger(Query query)
        {
            if (onTrigger == null)
                yield break;
            yield return onTrigger.Invoke(query);
        }

        public bool OnEnd(Query query)
        {
            if (onEnd == null)
                return true;
            return onEnd.Invoke(query);
        }
    }

    public class Query: IEnumerable<IAct>
    {
        List<IAct> acts;
        public Dictionary<IAct, UIEffectBubble> bubbles { get; private set; }
        public string lastTrigger;

        public Query(params IAct[] acts)
        {
            this.acts = new List<IAct>(acts);
            bubbles = new Dictionary<IAct, UIEffectBubble>(acts.Length);
        }

        public IEnumerator<IAct> GetEnumerator()
        {
            for (int i = 0; i < acts.Count; i++)
            {
                yield return acts[i];
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return acts.GetEnumerator();
        }

        public int Count { get => acts.Count; }

        public IAct this[int i] => acts[i];

        public void Add(IAct item) => acts.Add(item);

        public void RemoveAt(int i) => acts.RemoveAt(i);

        public void Remove(IAct item) => acts.Remove(item);

        public void Clear() => acts.Clear();

        public IEnumerator Invoke()
        {
            for (int i = 0; i < acts.Count; i++)
            {
                yield return acts[i].Activate();
            }
            acts.Clear();
            yield break;
        }

        public IEnumerator Process(string triggerSuffix)
        {
            yield return CardSystem.InvokeTrigger(this, "P" + triggerSuffix);
            yield return Invoke();
            yield return CardSystem.InvokeTrigger(this, "A" + triggerSuffix);
            yield return Invoke();
        }

        public bool TryGetBubble(IAct act, out UIEffectBubble bubble)
        {
            if (!bubbles.TryGetValue(act, out bubble))
                return false;
            if (!bubble.isLive)
                return false;
            return true;
        }

        public void AddBubble(IAct act, UIEffectBubble bubble)
        {
            bubbles[act] = bubble;
        }
    }

    public static class CardSystem
    {
        static List<Trigger> triggers = new List<Trigger>();

        public static void InitializeTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }

        public static bool TestTrigger(string trigger, string required)
        {
            if (trigger.Length < required.Length || required.Length == 0)
                return false;

            for (int i = 0; i < required.Length; i++)
            {
                if (trigger[i] != required[i] && required[i] != '_')
                    return false;
            }
            return true;
        }

        public static IEnumerator InvokeTrigger(Query query, string triggerStr)
        {
            query.lastTrigger = triggerStr;
            for (int i = 0; i < triggers.Count; i++)
            {
                Trigger trigger = triggers[i];
                if (TestTrigger(triggerStr, trigger.required))
                {
                    yield return trigger.OnTrigger(query);
                }

                if (TestTrigger(triggerStr, trigger.end) && trigger.OnEnd(query))
                {
                    triggers.RemoveAt(i);
                    i -= 1;
                }
            }
            yield break;
        }
    }
}
