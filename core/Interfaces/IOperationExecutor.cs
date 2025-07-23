namespace Recall.GameModel.Interfaces {
    using Recall.GameModel.Cards;
    using Recall.GameModel.Echo;

    /// <summary>
    /// Defines the interface for executing operation cards and replaying EchoCards.
    /// </summary>
    public interface IOperationExecutor {
        void Execute(CardInstance card);
        void ExecuteEcho(EchoCard echo);
    }
} 