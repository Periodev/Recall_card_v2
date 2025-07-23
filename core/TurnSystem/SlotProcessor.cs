using Recall.Combat;

namespace Recall.TurnSystem {
    public class SlotProcessor {
        public void ApplyStartOfTurn(Actor actor) {
            if (actor == null) throw new System.ArgumentNullException(nameof(actor));
            actor.AddShield(-actor.Shield); // 移除暫時護盾（Shield = 0）
        }

        public void ApplyEndOfTurn(Actor actor) {
            if (actor == null) throw new System.ArgumentNullException(nameof(actor));
            // 反傷（Thorns）
            if (actor.HasPassive("Thorns")) {
                actor.TakeDamage(1); // 範例：反傷 1 點
            }
            // 損血（Decay）
            if (actor.HasPassive("Decay")) {
                actor.TakeDamage(1); // 範例：損血 1 點
            }
        }
    }
} 