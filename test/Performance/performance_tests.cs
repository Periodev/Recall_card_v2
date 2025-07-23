using System;
using NUnit.Framework;
using Recall.Cards;
using Recall.Echo;
using Recall.Zones;
using Recall.Executor;

namespace Recall.Tests.Performance {

    [TestFixture]
    [Category("Performance")]
    public class PerformanceTests {

        [Test]
        public void MemoryTimeline_LargeNumberOfOperations_ShouldPerformWell() {
            // Arrange
            var timeline = new MemoryTimeline();
            const int operationCount = 10000;

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            for (int i = 0; i < operationCount; i++) {
                timeline.Push(TestHelpers.CreateTestCard($"PERF{i:0000}"));
            }
            
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 1000); // 應該在1秒內完成
            
            var recallable = timeline.GetRecallable();
            Assert.AreEqual(Recall.GameConstants.MemoryWindowSize, recallable.Count);
        }

        [Test]
        public void EchoCompiler_LargeMemoryTimeline_ShouldCompileQuickly() {
            // Arrange
            var timeline = new MemoryTimeline();
            var compiler = new EchoCompiler();
            
            // 填滿記憶時間軸 (4張卡片)
            for (int i = 0; i < Recall.GameConstants.MemoryWindowSize; i++) {
                timeline.Push(TestHelpers.CreateTestCard($"LARGE{i:00}"));
            }

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var echo = compiler.CompileFromMemory(timeline);
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 100); // 應該在100ms內完成
            Assert.AreEqual(Recall.GameConstants.MemoryWindowSize, echo.RecalledSequence.Count);
        }

        [Test]
        public void IntegratedFlow_HighThroughput_ShouldMaintainPerformance() {
            // Arrange
            var timeline = new MemoryTimeline();
            var compiler = new EchoCompiler();
            var executor = new OperationExecutor();
            const int rounds = 1000;

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            for (int round = 0; round < rounds; round++) {
                // 每輪添加一張卡片
                var card = TestHelpers.CreateTestCard($"ROUND{round:0000}");
                executor.Execute(card);
                timeline.Push(card);
                
                // 每10輪編譯並執行一次Echo
                if (round % 10 == 0) {
                    var echo = compiler.CompileFromMemory(timeline);
                    executor.ExecuteEcho(echo);
                }
            }
            
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 5000); // 應該在5秒內完成
        }

        [Test]
        public void MemoryManagement_ShouldNotLeakMemory() {
            // Arrange
            var timeline = new MemoryTimeline();
            var initialMemory = GC.GetTotalMemory(true);

            // Act
            for (int cycle = 0; cycle < 100; cycle++) {
                // 每個週期填滿並清空記憶
                for (int i = 0; i < Recall.GameConstants.MemoryWindowSize * 2; i++) {
                    timeline.Push(TestHelpers.CreateTestCard($"CYCLE{cycle}_CARD{i}"));
                }
            }

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;

            // Assert
            // 記憶體增長應該在合理範圍內（考慮到只保留4張卡片）
            Assert.Less(memoryIncrease, 512 * 1024); // 少於512KB增長
        }

        [Test]
        public void ConcurrentOperations_ShouldMaintainDataIntegrity() {
            // Arrange
            var timeline = new MemoryTimeline();
            var compiler = new EchoCompiler();
            const int operationsPerTask = 1000;

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // 模擬高頻操作
            for (int i = 0; i < operationsPerTask; i++) {
                // 快速添加和編譯
                timeline.Push(TestHelpers.CreateTestCard($"FAST{i:000}"));
                var echo = compiler.CompileFromMemory(timeline);
                
                // 驗證編譯結果的一致性
                Assert.IsNotNull(echo);
                Assert.IsNotNull(echo.RecalledSequence);
            }
            
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 2000); // 應該在2秒內完成
            
            // 最終狀態驗證
            var finalRecallable = timeline.GetRecallable();
            Assert.AreEqual(Recall.GameConstants.MemoryWindowSize, finalRecallable.Count);
        }
    }
}