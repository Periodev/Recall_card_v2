using Recall.Zones;
using Recall.Echo;

namespace Recall.Interfaces {
    /// <summary>
    /// Defines the interface for compiling an EchoCard from the memory timeline.
    /// </summary>
    public interface IEchoCompiler {
        EchoCard CompileFromMemory(MemoryTimeline timeline);
    }
} 