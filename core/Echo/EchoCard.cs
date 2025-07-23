using System.Collections.Generic;
using Recall.Cards;

namespace Recall.Echo {
    public class EchoCard {
        public List<CardInstance> RecalledSequence = new();
        public string ReverbSymbol; // e.g., final symbol "A"
    }
} 