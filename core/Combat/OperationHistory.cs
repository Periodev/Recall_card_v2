using System.Collections.Generic;
using Recall.Cards;

namespace Recall.Combat {
    public class OperationHistory {
        public List<CardInstance> AllActions = new();
        public void Record(CardInstance card) => AllActions.Add(card);
    }
} 