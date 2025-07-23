using System;
using System.Collections.Generic;

namespace Recall.Combat {
    public class Actor {
        public int HP { get; private set; }
        public int MaxHP { get; private set; }
        public int Shield { get; private set; }
        public HashSet<string> PassiveFlags { get; } = new();

        public Actor(int maxHP) {
            if (maxHP <= 0) throw new ArgumentOutOfRangeException(nameof(maxHP));
            MaxHP = maxHP;
            HP = maxHP;
        }

        public void TakeDamage(int amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            int damage = Math.Max(0, amount - Shield);
            Shield = Math.Max(0, Shield - amount);
            HP = Math.Max(0, HP - damage);
        }

        public void AddShield(int amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Shield += amount;
        }

        public void ApplyPassive(string flag) {
            if (string.IsNullOrEmpty(flag)) throw new ArgumentNullException(nameof(flag));
            PassiveFlags.Add(flag);
        }

        public bool HasPassive(string flag) {
            if (string.IsNullOrEmpty(flag)) return false;
            return PassiveFlags.Contains(flag);
        }
    }
} 