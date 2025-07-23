using System;
using System.Collections.Generic;
using Recall.Combat;
using Recall.Cards;
using Recall.Executor;

namespace Recall.TurnSystem {
    public class TurnManager {
        private readonly SlotProcessor slotProcessor = new();
        private readonly OperationExecutor executor = new();

        public void ExecutePlayerTurn(Actor player, List<CardInstance> hand) {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (hand == null) throw new ArgumentNullException(nameof(hand));
            slotProcessor.ApplyStartOfTurn(player);
            foreach (var card in hand) {
                executor.Execute(card);
            }
            slotProcessor.ApplyEndOfTurn(player);
        }

        public void ExecuteEnemyTurn(Actor enemy, EnemyBehavior behavior) {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            if (behavior == null) throw new ArgumentNullException(nameof(behavior));
            slotProcessor.ApplyStartOfTurn(enemy);
            var action = behavior.GetNextAction();
            if (action != null) {
                var instance = new CardInstance { CardData = action, InstanceId = Guid.NewGuid(), Source = CardSource.Deck };
                executor.Execute(instance);
            }
            slotProcessor.ApplyEndOfTurn(enemy);
        }
    }
} 