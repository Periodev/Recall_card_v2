using Recall.GameModel.Cards;
using Recall.GameModel.Echo;

namespace Recall.GameModel.Executor {
    public class OperationExecutor {
        public void Execute(CardInstance card) {
            // TODO: apply OperationEffect
        }

        public void ExecuteEcho(EchoCard echo) {
            foreach (var card in echo.RecalledSequence) {
                Execute(card);
            }
        }
    }
} 