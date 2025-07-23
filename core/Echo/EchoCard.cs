using System.Collections.Generic;
using Recall.GameModel.Cards;

namespace Recall.GameModel.Echo {
    public class EchoCard {
        public List<CardInstance> RecalledSequence = new();
        public string ReverbSymbol; // e.g., final symbol "A"
    }
} 