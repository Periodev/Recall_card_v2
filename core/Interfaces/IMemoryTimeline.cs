using System.Collections.Generic;
using Recall.Cards;

namespace Recall.Interfaces {
    /// <summary>
    /// Interface for accessing and manipulating the visible memory timeline.
    /// </summary>
    public interface IMemoryTimeline {
        void Push(CardInstance card);
        List<CardInstance> GetRecallable();
    }
} 