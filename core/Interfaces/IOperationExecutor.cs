using Recall.Cards;
using Recall.Echo;

namespace Recall.Interfaces {
    /// <summary>
    /// Defines the interface for executing operation cards and replaying EchoCards.
    /// </summary>
    public interface IOperationExecutor {
        void Execute(CardInstance card);
        void ExecuteEcho(EchoCard echo);
    }
} 