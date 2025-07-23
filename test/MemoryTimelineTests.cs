using NUnit.Framework;
using Recall.GameModel.Cards;
using Recall.GameModel.Zones;
using System.Collections.Generic;
using System.Linq;

namespace Recall.Tests {
    public class MemoryTimelineTests {
        private MemoryTimeline timeline;

        [SetUp]
        public void Setup() {
            const int maxSize = 3;
            timeline = new MemoryTimeline { MaxSize = maxSize };
        }

        private CardInstance MakeCard(string code, int damage = 5) {
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
        public void Push_ShouldTrimToMaxSize() {
            const int damage = 7;
            var inputCodes = new[] { "A", "B", "C", "D" };
            foreach (var code in inputCodes) {
                timeline.Push(MakeCard(code, damage));
            }

            var expected = new[] { "B", "C", "D" };
            var result = timeline.GetRecallable().Select(c => c.CardData.Code).ToArray();

            Assert.AreEqual(expected.Length, result.Length);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetRecallable_ShouldReturnInOrder() {
            const int damage = 3;
            var codes = new[] { "X", "Y", "Z" };

            foreach (var code in codes)
                timeline.Push(MakeCard(code, damage));

            var result = timeline.GetRecallable().Select(c => c.CardData.Code).ToList();
            CollectionAssert.AreEqual(codes, result);
        }

        [Test]
        public void EmptyTimeline_ShouldReturnEmptyList() {
            var recallable = timeline.GetRecallable();
            Assert.AreEqual(0, recallable.Count);
        }
    }
} 