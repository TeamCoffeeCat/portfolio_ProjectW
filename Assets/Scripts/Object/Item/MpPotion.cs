using ProjectW.SD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    public class MpPotion : Consume
    {
        public MpPotion(SDItem origin) : base(origin) { }
        
        public override void ItemEffect()
        {
            var character = GameManager.User.boCharacter;
            character.currentMana += value;

            if (character.currentMana > character.maxMana)
                character.currentMana = character.maxMana;

            IngameManager.Instance.playerMpValueChanged.Invoke(character.currentMana);
        }
    }
}