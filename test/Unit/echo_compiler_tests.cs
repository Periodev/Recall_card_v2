using System;
using NUnit.Framework;
using Recall.Cards;
using Recall.Echo;
using Recall.Zones;

namespace Recall.Tests.Unit {

    [TestFixture]
    public class EchoCompilerTests {
        private EchoCompiler compiler;
        private MemoryTimeline timeline;

        [SetUp]
        public void Setup() {
            compiler = new EchoCompiler();
            timeline = new MemoryTimeline();
        }

        [Test]
        public void CompileFromMemory_EmptyTimeline_ShouldReturnEmptyEcho() {
            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.IsNotNull(echo);
            Assert.IsNotNull(echo.RecalledSequence);
            Assert.AreEqual(0, echo.RecalledSequence.Count);
            Assert.IsNull(echo.ReverbSymbol);
        }

        [Test]
        public void CompileFromMemory_SingleCard_ShouldSetReverbSymbol() {
            // Arrange
            var card = TestHelpers.CreateTestCard("ATK01");
            timeline.Push(card);

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(1, echo.RecalledSequence.Count);
            Assert.AreEqual("ATK01", echo.ReverbSymbol);
            Assert.AreEqual("ATK01", echo.RecalledSequence[0].CardData.Code);
            Assert.AreEqual(card.InstanceId, echo.RecalledSequence[0].InstanceId);
        }

        [Test]
        public void CompileFromMemory_MultipleCards_ShouldUseLastCardAsReverb() {
            // Arrange
            var cards = TestHelpers.CreateTestCards("FIRST", "SECOND", "LAST");
            foreach (var card in cards) {
                timeline.Push(card);
            }

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(3, echo.RecalledSequence.Count);
            Assert.AreEqual("LAST", echo.ReverbSymbol);
        }

        [Test]
        public void CompileFromMemory_ShouldPreserveOrder() {
            // Arrange
            var expectedOrder = new[] { "ALPHA", "BETA", "GAMMA" };
            var cards = TestHelpers.CreateTestCards(expectedOrder);
            foreach (var card in cards) {
                timeline.Push(card);
            }

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            for (int i = 0; i < expectedOrder.Length; i++) {
                Assert.AreEqual(expectedOrder[i], 
                    echo.RecalledSequence[i].CardData.Code);
            }
        }

        [Test]
        public void CompileFromMemory_ShouldPreserveCardProperties() {
            // Arrange
            var attackCard = TestHelpers.CreateAttackCard("A", damage: 20, apCost: 3);
            attackCard.Source = CardSource.Echo; // 修改來源
            timeline.Push(attackCard);

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            var recalledCard = echo.RecalledSequence[0];
            Assert.AreEqual(attackCard.InstanceId, recalledCard.InstanceId);
            Assert.AreEqual(attackCard.Source, recalledCard.Source);
            Assert.AreEqual(attackCard.CardData.Type, recalledCard.CardData.Type);
            Assert.AreEqual(attackCard.CardData.APCost, recalledCard.CardData.APCost);
            Assert.AreEqual(attackCard.CardData.Effect.Damage, recalledCard.CardData.Effect.Damage);
        }

        [Test]
        public void CompileFromMemory_NullTimeline_ShouldThrowException() {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                compiler.CompileFromMemory(null));
        }

        [Test]
        public void CompileFromMemory_MultipleCompilations_ShouldReturnConsistentResults() {
            // Arrange
            timeline.Push(TestHelpers.CreateTestCard("STABLE"));

            // Act
            var echo1 = compiler.CompileFromMemory(timeline);
            var echo2 = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(echo1.RecalledSequence.Count, echo2.RecalledSequence.Count);
            Assert.AreEqual(echo1.ReverbSymbol, echo2.ReverbSymbol);
            
            // 但是應該是不同的 EchoCard 實例
            Assert.AreNotSame(echo1, echo2);
            Assert.AreNotSame(echo1.RecalledSequence, echo2.RecalledSequence);
        }

        [Test]
        public void CompileFromMemory_FullMemoryWindow_ShouldCompileAllCards() {
            // Arrange
            var maxSize = Recall.GameConstants.MemoryWindowSize; // 實際值是 4
            for (int i = 0; i < maxSize; i++) {
                timeline.Push(TestHelpers.CreateTestCard($"FULL{i:00}"));
            }

            // Act
            var echo = compiler.CompileFromMemory(timeline);

            // Assert
            Assert.AreEqual(maxSize, echo.RecalledSequence.Count);
            Assert.AreEqual("FULL03", echo.ReverbSymbol); // maxSize - 1 = 3
        }
    }
}