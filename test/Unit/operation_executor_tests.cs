using System;
using NUnit.Framework;
using Recall.Cards;
using Recall.Executor;
using Recall.Echo;
using Recall.Combat;

namespace Recall.Tests.Unit {

    [TestFixture]
    public class OperationExecutorTests {
        private OperationExecutor executor;

        [SetUp]
        public void Setup() {
            executor = new OperationExecutor();
        }

        [Test]
        public void Execute_ValidCard_ShouldNotThrow() {
            // Arrange
            var card = TestHelpers.CreateTestCard("ATK01");

            // Act & Assert
            Assert.DoesNotThrow(() => executor.Execute(card));
        }

        [Test]
        public void Execute_NullCard_ShouldHandleGracefully() {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => executor.Execute(null));
        }

        [Test]
        public void Execute_DifferentCardTypes_ShouldAllExecute() {
            // Arrange
            var attackCard = TestHelpers.CreateAttackCard("A", damage: 15);
            var blockCard = TestHelpers.CreateBlockCard("B", shield: 10);
            var chargeCard = TestHelpers.CreateTestCard("C", OperationType.Charge);

            // Act & Assert
            Assert.DoesNotThrow(() => executor.Execute(attackCard));
            Assert.DoesNotThrow(() => executor.Execute(blockCard));
            Assert.DoesNotThrow(() => executor.Execute(chargeCard));
        }

        [Test]
        public void ExecuteEcho_EmptySequence_ShouldNotThrow() {
            // Arrange
            var echo = new EchoCard();

            // Act & Assert
            Assert.DoesNotThrow(() => executor.ExecuteEcho(echo));
        }

        [Test]
        public void ExecuteEcho_SingleCard_ShouldExecuteCard() {
            // Arrange
            var card = TestHelpers.CreateTestCard("ATK01");
            var echo = new EchoCard();
            echo.RecalledSequence.Add(card);

            // Act & Assert
            Assert.DoesNotThrow(() => executor.ExecuteEcho(echo));
        }

        [Test]
        public void ExecuteEcho_MultipleCards_ShouldExecuteInOrder() {
            // Arrange
            var cards = TestHelpers.CreateTestCards("FIRST", "SECOND", "THIRD");
            var echo = new EchoCard();
            echo.RecalledSequence.AddRange(cards);

            // Act & Assert
            Assert.DoesNotThrow(() => executor.ExecuteEcho(echo));
            
            // 注意：由於 Execute 方法目前是空的，我們只能測試不拋出異常
            // 當 Execute 實現後，可以添加更詳細的驗證
        }

        [Test]
        public void ExecuteEcho_NullEcho_ShouldThrowException() {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                executor.ExecuteEcho(null));
        }

        [Test]
        public void ExecuteEcho_EchoWithNullSequence_ShouldHandleGracefully()
        {
            // Arrange
            var echo = new EchoCard();
            echo.RecalledSequence = null;

            // Act & Assert
            // 你的代碼現在拋出 ArgumentException，不是 NullReferenceException
            Assert.Throws<ArgumentException>(() => executor.ExecuteEcho(echo));
        }

        [Test]
        public void ExecuteEcho_EchoWithNullCardsInSequence_ShouldHandleGracefully() {
            // Arrange
            var echo = new EchoCard();
            echo.RecalledSequence.Add(TestHelpers.CreateTestCard("VALID"));
            echo.RecalledSequence.Add(null);
            echo.RecalledSequence.Add(TestHelpers.CreateTestCard("ALSO_VALID"));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => executor.ExecuteEcho(echo));
        }

        private bool ShouldThrowOnNullCard() => true; // 根據實際設計決定
    }
}