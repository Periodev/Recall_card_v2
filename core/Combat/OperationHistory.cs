using System.Collections.Generic;
using Recall.GameModel.Cards;

namespace Recall.GameModel.Combat {
    public class OperationHistory {
        public List<CardInstance> AllActions = new();
        public void Record(CardInstance card) => AllActions.Add(card);
    }
} 