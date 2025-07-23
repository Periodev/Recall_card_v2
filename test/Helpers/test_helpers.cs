using System;
using System.Collections.Generic;
using System.Linq;
using Recall.Cards;

namespace Recall.Tests {

    public static class TestHelpers {
        public static CardInstance CreateTestCard(string code, OperationType type = OperationType.Attack, 
            int apCost = 1, CardSource source = CardSource.Deck) {
            return new CardInstance {
                CardData = new OperationCard { 
                    Code = code, 
                    Type = type,
                    APCost = apCost,
                    Effect = new OperationEffect { Damage = 10, Shield = 0, ChargeBonus = 0 }
                },
                InstanceId = Guid.NewGuid(),
                Source = source
            };
        }

        public static List<CardInstance> CreateTestCards(params string[] codes) {
            return codes.Select(code => CreateTestCard(code)).ToList();
        }
        
        public static CardInstance CreateAttackCard(string code, int damage = 10, int apCost = 1) {
            return new CardInstance {
                CardData = new OperationCard {
                    Code = code,
                    Type = OperationType.Attack,
                    APCost = apCost,
                    Effect = new OperationEffect { Damage = damage, Shield = 0, ChargeBonus = 0 }
                },
                InstanceId = Guid.NewGuid(),
                Source = CardSource.Deck
            };
        }
        
        public static CardInstance CreateBlockCard(string code, int shield = 5, int apCost = 1) {
            return new CardInstance {
                CardData = new OperationCard {
                    Code = code,
                    Type = OperationType.Block,
                    APCost = apCost,
                    Effect = new OperationEffect { Damage = 0, Shield = shield, ChargeBonus = 0 }
                },
                InstanceId = Guid.NewGuid(),
                Source = CardSource.Deck
            };
        }
    }
}