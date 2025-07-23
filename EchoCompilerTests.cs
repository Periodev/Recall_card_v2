
using NUnit.Framework;
using Recall.GameModel.Cards;
using Recall.GameModel.Zones;
using Recall.GameModel.Echo;
using System.Collections.Generic;
using System.Linq;

namespace Recall.Tests {
    public class EchoCompilerTests {
        private MemoryTimeline timeline;
        private EchoCompiler compiler;

        [SetUp]
        public void Setup() {
            const int maxSize = 4;
            timeline = new MemoryTimeline { MaxSize = maxSize };
            compiler = new EchoCompiler();
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
        public void CompileFromMemory_ShouldReturnFullSequence() {
            const int damage = 10;
            var codes = new[] { "A", "B", "C" };
            foreach (var code in codes)
                timeline.Push(MakeCard(code, damage));

            var echo = compiler.CompileFromMemory(timeline);

            Assert.NotNull(echo);
            Assert.AreEqual(codes.Length, echo.RecalledSequence.Count);
            CollectionAssert.AreEqual(codes, echo.RecalledSequence.Select(c => c.CardData.Code));
        }

        [Test]
        public void CompileFromMemory_ShouldSetReverbSymbolToLastCode() {
            const int damage = 5;
            var codes = new[] { "X", "Y" };
            foreach (var code in codes)
                timeline.Push(MakeCard(code, damage));

            var echo = compiler.CompileFromMemory(timeline);
            Assert.AreEqual("Y", echo.ReverbSymbol);
        }

        [Test]
        public void CompileFromMemory_EmptyTimeline_ShouldReturnEmptyEcho() {
            var echo = compiler.CompileFromMemory(timeline);

            Assert.NotNull(echo);
            Assert.AreEqual(0, echo.RecalledSequence.Count);
            Assert.IsNull(echo.ReverbSymbol);
        }
    }
}
