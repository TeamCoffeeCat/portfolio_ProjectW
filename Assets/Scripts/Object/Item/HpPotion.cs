using ProjectW.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    public class HpPotion : Consume
    {
        public HpPotion(SDItem origin) : base(origin) { }
        
        public override void ItemEffect()
        {
            var character = GameManager.User.boCharacter;
            character.currentHp += value;
            
            if (character.currentHp > character.maxHp)
                character.currentHp = character.maxHp;

            IngameManager.Instance.playerHpValueChanged.Invoke(character.currentHp);
        }
    }
}