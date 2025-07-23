using System.Collections.Generic;
using Recall.Cards;

namespace Recall.TurnSystem {
    public class EnemyBehavior {
        private readonly Queue<OperationCard> actions = new();
        public EnemyBehavior(IEnumerable<OperationCard> sequence) {
            if (sequence != null) {
                foreach (var card in sequence) actions.Enqueue(card);
            }
        }
        public OperationCard GetNextAction() {
            if (actions.Count == 0) return null;
            var card = actions.Dequeue();
            actions.Enqueue(card); // 循環腳本
            return card;
        }
    }

    public static class EnemyFactory {
        public static EnemyBehavior CreateDefaultEnemy() {
            return new EnemyBehavior(new[] {
                new OperationCard { Code = "A", Type = OperationType.Attack, APCost = 1, Effect = new OperationEffect { Damage = 5 } },
                new OperationCard { Code = "B", Type = OperationType.Block, APCost = 1, Effect = new OperationEffect { Shield = 3 } },
                new OperationCard { Code = "C", Type = OperationType.Charge, APCost = 1, Effect = new OperationEffect { ChargeBonus = 2 } },
            });
        }
    }
} 