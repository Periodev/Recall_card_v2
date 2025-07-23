using System;
using Recall.Cards;
using Recall.Echo;

namespace Recall.Executor {
    public class OperationExecutor {
        public void Execute(CardInstance card) {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            // TODO: apply OperationEffect
        }

        public void ExecuteEcho(EchoCard echo) {
            if (echo == null)
                throw new ArgumentNullException(nameof(echo));
            if (echo.RecalledSequence == null)
                throw new ArgumentException("RecalledSequence cannot be null", nameof(echo));
            foreach (var card in echo.RecalledSequence) {
                if (card == null)
                    throw new ArgumentException("RecalledSequence contains null card", nameof(echo));
                Execute(card);
            }
        }
    }
} 