using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ActSystem
{
    public abstract class Act
    {
        public bool cancel = false;
        public readonly string triggerWord;
        private UIEffectBubble effect;
        public CardInstance instance { get; private set; }

        public Act(string triggerWord)
        {
            this.triggerWord = triggerWord;
            effect = null;
        }

        public IEnumerator Invoke(CardInstance instance)
        {
            this.instance = instance;

            yield return CardSystem.InvokeTrigger(this, "B/" + triggerWord);

            if (cancel)
                yield break;

            Activate();

            yield return CardSystem.InvokeTrigger(this, "A/" + triggerWord);
        }

        protected virtual void Activate()
        {

        }

        public void AddEffect(UIEffectBubble effect)
        {
            this.effect = effect;
        }

        public bool TryGetEffect(out UIEffectBubble effect)
        {
            effect = this.effect;
            return this.effect != null;
        }
    }

    public class Trigger
    {
        public readonly string required;
        public readonly Func<Act, bool> requiredCondition;
        public readonly string end;
        public readonly Func<Act, bool> endCondition;
        public readonly Func<Act, IEnumerator> onTrigger;

        private bool useInstance = false;
        private CardInstance instance;

        public Trigger(string required, Func<Act, bool> requiredCondition, string end, Func<Act, bool> endCondition, Func<Act, IEnumerator> onTrigger)
        {
            this.required = required;
            this.requiredCondition = requiredCondition;
            this.end = end;
            this.endCondition = endCondition;
            this.onTrigger = onTrigger;
        }

        public Trigger SetLink(CardInstance instance)
        {
            this.instance = instance;
            useInstance = true;
            return this;
        }

        public bool IsAlive()
        {
            return !useInstance || instance.isAlive;
        }
    }

    public static class CardSystem
    {
        public static readonly List<Trigger> triggers = new List<Trigger>();

        static int triggerCount;

        public static void Reset()
        {
            triggers.Clear();
        }

        public static void ResetTriggerCount(int count)
        {
            triggerCount = count;
        }

        public static void InitializeTrigger(Trigger trigger)
        {
            triggers.Add(trigger);
        }

        public static bool TestTrigger(string triggerWord, string required)
        {
            if (triggerWord.Length < required.Length || required.Length == 0)
                return false;

            for (int i = 0; i < required.Length; i++)
            {
                if (triggerWord[i] != required[i] && required[i] != '_')
                    return false;
            }
            return true;
        }

        public static IEnumerator InvokeTrigger(Act act, string triggerWord)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (act != null && act.cancel)
                    yield break;

                Trigger trigger = triggers[i];
                if (trigger.IsAlive() && TestTrigger(triggerWord, trigger.required) && (trigger.requiredCondition == null || trigger.requiredCondition(act)))
                {
                    if (triggerCount <= 0)
                    {
                        string content = Utils.GetLocalizedString("trigger_excution_reached_maximum");
                        GameManager.Instance.card.field.Alert(content);
                        yield break;
                    }
                    triggerCount -= 1;

                    yield return trigger.onTrigger(act);
                }

                if (triggers.Contains(trigger) && !trigger.IsAlive() || (TestTrigger(triggerWord, trigger.end) && (trigger.endCondition == null || trigger.endCondition(act))))
                {
                    triggers.RemoveAt(i);
                    i -= 1;
                }
            }
            yield break;
        }
    }

    //-- Acts --//
    public class AddTrigger: Act
    {
        public readonly Trigger trigger;

        public AddTrigger(string t, Trigger trigger): base(t)
        {
            this.trigger = trigger;
        }

        protected override void Activate()
        {
            CardSystem.InitializeTrigger(trigger);
        }
    }

    public class AddHealth: Act
    {
        public int amount;
        public Entity target;

        public AddHealth(string t, int amount, Entity target): base(t)
        {
            this.amount = amount;
            this.target = target;
        }

        protected override void Activate()
        {
            target.health += amount;
        }
    }

    public class Rotate: Act
    {
        public readonly Rotation r;

        public Rotate(string t, Rotation r): base(t)
        {
            this.r = r;
        }

        protected override void Activate()
        {
            instance.field.currentDir = instance.field.currentDir.Rotate(r);
        }
    }

    public class SetIntensity: Act
    {
        public readonly CardInstance target;
        public readonly int newIntensity;

        public SetIntensity(string t, CardInstance target, int newIntensity): base(t)
        {
            this.target = target;
            this.newIntensity = newIntensity;
        }

        protected override void Activate()
        {
            target.intensity = newIntensity;
            var item = instance.field.GetItem(instance.pos);
            item.UpdateUI();
        }
    }

    public class Del: Act
    {
        public readonly CardInstance target;

        public Del(string t, CardInstance target): base(t)
        {
            this.target = target;
        }

        protected override void Activate()
        {
            var item = instance.field.GetItem(target.pos);
            item.SetData(null, EntityType.P);
            instance.isAlive = false;
        }
    }

    public class Remain: Act
    {
        public readonly CardInstance target;
        public readonly bool value;

        public Remain(string t, CardInstance target, bool value): base(t)
        {
            this.target = target;
            this.value = value;
        }

        protected override void Activate()
        {
            target.isRemaining = value;
        }
    }
}
