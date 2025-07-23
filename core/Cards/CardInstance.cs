namespace Recall.Cards {
    public enum CardSource { Deck, Echo, Generated }

    public class CardInstance {
        public OperationCard CardData;
        public Guid InstanceId;
        public CardSource Source;
    }
} 