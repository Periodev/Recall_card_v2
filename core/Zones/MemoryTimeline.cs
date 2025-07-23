using System.Collections.Generic;
using Recall.GameModel.Cards;

namespace Recall.GameModel.Zones {
    public class MemoryTimeline {
        private readonly Queue<CardInstance> memoryWindow = new();
        public int MaxSize = GameConstants.MemoryWindowSize;

        public void Push(CardInstance card) {
            if (memoryWindow.Count >= MaxSize)
                memoryWindow.Dequeue();
            memoryWindow.Enqueue(card);
        }

        public List<CardInstance> GetRecallable() => new(memoryWindow);
    }
} 