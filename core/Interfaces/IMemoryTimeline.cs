namespace Recall.GameModel.Interfaces {
    using Recall.GameModel.Cards;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for accessing and manipulating the visible memory timeline.
    /// </summary>
    public interface IMemoryTimeline {
        void Push(CardInstance card);
        List<CardInstance> GetRecallable();
    }
} 