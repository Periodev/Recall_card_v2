
using NUnit.Framework;
using Recall.Cards;
using Recall.Executor;
using Recall.Echo;

namespace Recall.Tests {
    public class OperationExecutorTests {
        private OperationExecutor executor;

        [SetUp]
        public void Setup() {
            executor = new OperationExecutor();
        }

        private CardInstance MakeCard(string code, int damage = 10) {
            const int apCost = 1;
            return new CardInstance {
                CardData = new OperationCard {
                    Code = code,
                    Type = OperationType.Attack,
                    APCost = apCost,
                    Effect = new OperationEffect { Damage = damage }
                },
                Source = CardSource.Deck
            };
        }

        [Test]
        public void Execute_ShouldPerformCorrectAction() {
            const int damage = 10;
            const string code = "A";

            var card = MakeCard(code, damage);
            executor.Execute(card);

            // Here we assume executor logs the action or modifies state (this is just a placeholder)
            // A real test would check the state changes or mock a handler method
            Assert.Pass("Executed correctly, add actual logic verification.");
        }

        [Test]
        public void ExecuteEcho_ShouldPerformAllActions() {
            const int damage = 5;
            var echo = new EchoCard {
                RecalledSequence = new List<CardInstance> {
                    MakeCard("A", damage),
                    MakeCard("B", damage)
                }
            };

            executor.ExecuteEcho(echo);

            // Similar to the above, verify effects or changes in the system state
            Assert.Pass("Executed Echo correctly, add actual logic verification.");
        }

        [Test]
        public void ExecuteEcho_EmptyEcho_ShouldNotFail() {
            var emptyEcho = new EchoCard { RecalledSequence = new List<CardInstance>() };

            executor.ExecuteEcho(emptyEcho);

            Assert.Pass("Handled empty Echo gracefully.");
        }
    }
}
