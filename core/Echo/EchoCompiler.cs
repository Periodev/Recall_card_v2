using System;
using Recall.Zones;

namespace Recall.Echo {
    public class EchoCompiler {
        public EchoCard CompileFromMemory(MemoryTimeline timeline) {
            if (timeline == null)
                throw new ArgumentNullException(nameof(timeline));

            var echo = new EchoCard();
            echo.RecalledSequence = timeline.GetRecallable();
            if (echo.RecalledSequence.Count > 0)
                echo.ReverbSymbol = echo.RecalledSequence[^1].CardData.Code;
            return echo;
        }
    }
} 