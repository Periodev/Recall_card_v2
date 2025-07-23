using System;
using System.Collections.Generic;
using NUnit.Framework;
using Recall.Cards;
using Recall.Zones;

namespace Recall.Tests.Unit {

    [TestFixture]
    public class MemoryTimelineTests {
        private MemoryTimeline timeline;

        [SetUp]
        public void Setup() {
            timeline = new MemoryTimeline();
        }

        [Test]
        public void Push_SingleCard_ShouldAddToMemory() {
            // Arrange
            var card = TestHelpers.CreateTestCard("ATK01");

            // Act
            timeline.Push(card);
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(1, recallable.Count);
            Assert.AreEqual("ATK01", recallable[0].CardData.Code);
            Assert.AreEqual(card.InstanceId, recallable[0].InstanceId);
        }

        [Test]
        public void Push_ExceedsMaxSize_ShouldRemoveOldest() {
            // Arrange
            var maxSize = Recall.GameConstants.MemoryWindowSize; // 使用實際的常數值 4
            var cards = new List<CardInstance>();
            
            // 創建超過最大容量的卡片 (4 + 2 = 6 張)
            for (int i = 0; i < maxSize + 2; i++) {
                cards.Add(TestHelpers.CreateTestCard($"CARD{i:00}"));
            }

            // Act
            foreach (var card in cards) {
                timeline.Push(card);
            }
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(maxSize, recallable.Count);
            // 應該保留最後 4 張卡片：CARD02, CARD03, CARD04, CARD05
            Assert.AreEqual("CARD02", recallable[0].CardData.Code); // 最舊的應該是 CARD02
            Assert.AreEqual("CARD05", recallable[^1].CardData.Code); // 最新的應該是 CARD05
        }

        [Test]
        public void Push_AtMaxCapacity_ShouldMaintainOrder() {
            // Arrange
            var cards = TestHelpers.CreateTestCards("FIRST", "SECOND", "THIRD");

            // Act
            foreach (var card in cards) {
                timeline.Push(card);
            }
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(3, recallable.Count);
            Assert.AreEqual("FIRST", recallable[0].CardData.Code);
            Assert.AreEqual("SECOND", recallable[1].CardData.Code);
            Assert.AreEqual("THIRD", recallable[2].CardData.Code);
        }

        [Test]
        public void GetRecallable_EmptyTimeline_ShouldReturnEmptyList() {
            // Act
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.IsNotNull(recallable);
            Assert.AreEqual(0, recallable.Count);
        }

        [Test]
        public void GetRecallable_ShouldReturnCopy() {
            // Arrange
            timeline.Push(TestHelpers.CreateTestCard("TEST"));

            // Act
            var recallable1 = timeline.GetRecallable();
            var recallable2 = timeline.GetRecallable();

            // Assert
            Assert.AreNotSame(recallable1, recallable2); // 應該是不同的實例
            Assert.AreEqual(recallable1.Count, recallable2.Count);
        }

        [Test]
        public void Push_NullCard_ShouldHandleGracefully() {
            // 測試 null 卡片的處理 - 根據實際需求決定預期行為
            Assert.Throws<ArgumentNullException>(() => timeline.Push(null));
        }

        [Test]
        public void Push_DifferentCardTypes_ShouldAllBeStored() {
            // Arrange
            var attackCard = TestHelpers.CreateAttackCard("A", damage: 15);
            var blockCard = TestHelpers.CreateBlockCard("B", shield: 10);
            var chargeCard = TestHelpers.CreateTestCard("C", OperationType.Charge, apCost: 2);

            // Act
            timeline.Push(attackCard);
            timeline.Push(blockCard);
            timeline.Push(chargeCard);
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(3, recallable.Count);
            Assert.AreEqual(OperationType.Attack, recallable[0].CardData.Type);
            Assert.AreEqual(OperationType.Block, recallable[1].CardData.Type);
            Assert.AreEqual(OperationType.Charge, recallable[2].CardData.Type);
            
            // 驗證效果也被正確保存
            Assert.AreEqual(15, recallable[0].CardData.Effect.Damage);
            Assert.AreEqual(10, recallable[1].CardData.Effect.Shield);
            Assert.AreEqual(2, recallable[2].CardData.APCost);
        }

        [Test]
        public void MemoryTimeline_WithMaxSizeOne_ShouldWorkCorrectly() {
            // 需要修改 MemoryTimeline 以支援自訂 MaxSize
            timeline.MaxSize = 1;

            // Act
            timeline.Push(TestHelpers.CreateTestCard("FIRST"));
            timeline.Push(TestHelpers.CreateTestCard("SECOND"));
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(1, recallable.Count);
            Assert.AreEqual("SECOND", recallable[0].CardData.Code);
        }

        [Test]
        public void MemoryTimeline_WithZeroMaxSize_ShouldThrowException()
        {
            // Act & Assert
            // 如果你的 MemoryTimeline 沒有建構函數，需要用屬性設定
            var newTimeline = new MemoryTimeline();
            Assert.Throws<ArgumentOutOfRangeException>(() => newTimeline.MaxSize = 0);

            // 或者如果你已經加了建構函數：
            // Assert.Throws<ArgumentOutOfRangeException>(() => new MemoryTimeline(0));        
        }

        [Test]
        public void Push_SameCardMultipleTimes_ShouldStoreSeparateInstances() {
            // Arrange
            var originalCard = TestHelpers.CreateTestCard("SAME");
            var duplicateCard = TestHelpers.CreateTestCard("SAME"); // 不同 InstanceId

            // Act
            timeline.Push(originalCard);
            timeline.Push(duplicateCard);
            var recallable = timeline.GetRecallable();

            // Assert
            Assert.AreEqual(2, recallable.Count);
            Assert.AreEqual("SAME", recallable[0].CardData.Code);
            Assert.AreEqual("SAME", recallable[1].CardData.Code);
            Assert.AreNotEqual(recallable[0].InstanceId, recallable[1].InstanceId);
        }

        private bool ShouldThrowOnNull() => true; // 根據實際設計決定
    }
}