using System.Collections.Generic;
using Recall.Cards;

namespace Recall.Zones {
    public class HandZone {
        public List<CardInstance> Hand = new();
        public void Add(CardInstance card) => Hand.Add(card);
        public void Remove(CardInstance card) => Hand.Remove(card);
    }
} 