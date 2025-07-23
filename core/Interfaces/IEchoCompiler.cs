namespace Recall.GameModel.Interfaces {
    using Recall.GameModel.Zones;
    using Recall.GameModel.Echo;

    /// <summary>
    /// Defines the interface for compiling an EchoCard from the memory timeline.
    /// </summary>
    public interface IEchoCompiler {
        EchoCard CompileFromMemory(MemoryTimeline timeline);
    }
} 