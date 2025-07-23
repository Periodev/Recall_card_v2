namespace Recall.Cards {
    /// <summary>
    /// Enum representing the core types of operations a player can perform.
    /// </summary>
    public enum OperationType {
        Attack,     // A - 攻擊
        Block,      // B - 格擋
        Charge,     // C - 蓄力
        Clear       // D - 清除
    }

    /// <summary>
    /// Defines the effect payload carried by an OperationCard.
    /// </summary>
    public class OperationEffect {
        public int Damage { get; set; } = 0;
        public int Shield { get; set; } = 0;
        public int ChargeBonus { get; set; } = 0;

        public override string ToString() {
            return $"D:{Damage} / S:{Shield} / C:{ChargeBonus}";
        }
    }
} 