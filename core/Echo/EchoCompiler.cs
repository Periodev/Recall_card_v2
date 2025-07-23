using Recall.GameModel.Zones;

namespace Recall.GameModel.Echo {
    public class EchoCompiler {
        public EchoCard CompileFromMemory(MemoryTimeline timeline) {
            var echo = new EchoCard();
            echo.RecalledSequence = timeline.GetRecallable();
            if (echo.RecalledSequence.Count > 0)
                echo.ReverbSymbol = echo.RecalledSequence[^1].CardData.Code;
            return echo;
        }
    }
} 