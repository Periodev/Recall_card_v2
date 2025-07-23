using System;
using System.Collections.Generic;
using Recall.Cards;

namespace Recall.Zones {
    public class MemoryTimeline {
        private readonly Queue<CardInstance> memoryWindow = new();
        private int _maxSize;

        public int MaxSize {
            get => _maxSize;
            set {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(MaxSize), "MaxSize must be positive");
                _maxSize = value;
            }
        }

        public MemoryTimeline(int maxSize = GameConstants.MemoryWindowSize) {
            MaxSize = maxSize;
        }

        public void Push(CardInstance card) {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            if (MaxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(MaxSize), "MaxSize must be positive");
            if (memoryWindow.Count >= MaxSize)
                memoryWindow.Dequeue();
            memoryWindow.Enqueue(card);
        }

        public List<CardInstance> GetRecallable() => new(memoryWindow);
    }
} 