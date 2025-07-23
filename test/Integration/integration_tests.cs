using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Recall.Cards;
using Recall.Echo;
using Recall.Zones;
using Recall.Executor;

namespace Recall.Tests.Integration {

    [TestFixture]
    public class IntegrationTests {
        private MemoryTimeline timeline;
        private EchoCompiler compiler;
        private OperationExecutor executor;

        [SetUp]
        public void Setup() {
            timeline = new MemoryTimeline();
            compiler = new EchoCompiler();
            executor = new OperationExecutor();
        }

        [Test]
        public void CompleteEchoFlow_ShouldWorkEndToEnd() {
            // Arrange
            var originalCards = TestHelpers.CreateTestCards("ATK01", "DEF02", "SKILL03");

            // Act - 執行原始卡片並加入記憶
            foreach (var card in originalCards) {
                executor.Execute(card);
                timeline.Push(card);
            }

            // 編譯Echo並執行
            var echo = compiler.CompileFromMemory(timeline);
            
            // Assert
            Assert.AreEqual(3, echo.RecalledSequence.Count);
            Assert.AreEqual("SKILL03", echo.ReverbSymbol);
            
            // 執行Echo應該不會拋出異常
            Assert.DoesNotThrow(() => executor.ExecuteEcho(echo));
        }

        [Test]
        public void MemoryOverflow_ShouldStillCompileCorrectly() {
            // Arrange - 創建超過記憶容量的卡片
            var maxSize = Recall.GameConstants.MemoryWindowSize; // 實際值是 4
            var allCards = new List<CardInstance>();
            
            for (int i = 0; i < maxSize + 3; i++) { // 總共 7 張卡片
                var card = TestHelpers.CreateTestCard($"CARD{i:00}");
                allCards.Add(card);
                executor.Execute(card);
                timeline.Push(card);
            }

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(maxSize, echo.RecalledSequence.Count);
            // 應該包含最後 4 張卡片：CARD03, CARD04, CARD05, CARD06
            Assert.AreEqual("CARD06", echo.ReverbSymbol);
            
            // 驗證包含的卡片是正確的
            var expectedCodes = new[] { "CARD03", "CARD04", "CARD05", "CARD06" };
            for (int i = 0; i < maxSize; i++) {
                Assert.AreEqual(expectedCodes[i], 
                    echo.RecalledSequence[i].CardData.Code);
            }
        }

        [Test]
        public void MultipleEchoRounds_ShouldMaintainConsistency() {
            // Arrange
            var initialCards = TestHelpers.CreateTestCards("INIT1", "INIT2");

            // Act - 第一輪
            foreach (var card in initialCards) {
                executor.Execute(card);
                timeline.Push(card);
            }

            var firstEcho = compiler.CompileFromMemory(timeline);
            executor.ExecuteEcho(firstEcho);

            // 第二輪 - 添加更多卡片
            var secondRoundCards = TestHelpers.CreateTestCards("ROUND2_1", "ROUND2_2");
            foreach (var card in secondRoundCards) {
                executor.Execute(card);
                timeline.Push(card);
            }

            var secondEcho = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(4, secondEcho.RecalledSequence.Count);
            Assert.AreEqual("ROUND2_2", secondEcho.ReverbSymbol);
            
            // 應該包含所有四張卡片
            var expectedCodes = new[] { "INIT1", "INIT2", "ROUND2_1", "ROUND2_2" };
            for (int i = 0; i < expectedCodes.Length; i++) {
                Assert.AreEqual(expectedCodes[i], 
                    secondEcho.RecalledSequence[i].CardData.Code);
            }
        }

        [Test]
        public void EchoOfEchoCards_ShouldPreserveSource() {
            // Arrange
            var originalCard = TestHelpers.CreateTestCard("ORIGINAL", source: CardSource.Deck);
            var echoSourceCard = TestHelpers.CreateTestCard("ECHO_SOURCE", source: CardSource.Echo);

            // Act
            timeline.Push(originalCard);
            timeline.Push(echoSourceCard);
            
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(2, echo.RecalledSequence.Count);
            Assert.AreEqual(CardSource.Deck, echo.RecalledSequence[0].Source);
            Assert.AreEqual(CardSource.Echo, echo.RecalledSequence[1].Source);
        }

        [Test]
        public void DifferentCardTypes_ShouldAllBeEchoed() {
            // Arrange
            var attackCard = TestHelpers.CreateAttackCard("A", damage: 15);
            var blockCard = TestHelpers.CreateBlockCard("B", shield: 10);
            var chargeCard = TestHelpers.CreateTestCard("C", OperationType.Charge);

            // Act
            timeline.Push(attackCard);
            timeline.Push(blockCard);
            timeline.Push(chargeCard);
            
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(3, echo.RecalledSequence.Count);
            Assert.AreEqual(OperationType.Attack, echo.RecalledSequence[0].CardData.Type);
            Assert.AreEqual(OperationType.Block, echo.RecalledSequence[1].CardData.Type);
            Assert.AreEqual(OperationType.Charge, echo.RecalledSequence[2].CardData.Type);
            
            // Echo 執行應該成功
            Assert.DoesNotThrow(() => executor.ExecuteEcho(echo));
        }
    }
}