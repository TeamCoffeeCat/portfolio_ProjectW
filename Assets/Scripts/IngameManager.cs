using ProjectW.Object;
using ProjectW.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeCat;
using UnityEngine.Events;
using UnityEngine;
using CoffeeCat.Simplify;

namespace ProjectW
{
    /// <summary>
    /// 인게임을 제어할 클래스
    /// </summary>
    public class IngameManager : Singleton<IngameManager>
    {
        public Character character;
        private float fallDamage = 20f;

        public UnityEvent<float> playerHpValueChanged = new UnityEvent<float>();
        public UnityEvent<float> playerMpValueChanged = new UnityEvent<float>();
        public UnityEvent<float> playerExpValueChanged = new UnityEvent<float>();
        public UnityEvent<float> playerLevelValueChanged = new UnityEvent<float>();

        public UnityEvent<string> AddItemSystemMessage = new UnityEvent<string>();

        public void Init(Character character)
        {
            this.character = character;
        }

        public void AddEventToIngameUIValueChanged(UnityAction<float> action, IngameUIType value)
        {
            switch (value)
            {
                case IngameUIType.Hp:
                    playerHpValueChanged.AddListener(action);
                    break;
                case IngameUIType.Mp:
                    playerMpValueChanged.AddListener(action);
                    break;
                case IngameUIType.Exp:
                    playerExpValueChanged.AddListener(action);
                    break;
                case IngameUIType.Level:
                    playerLevelValueChanged.AddListener(action);
                    break;
            }
        }

        public void RemoveEventToIngameUIValueChanged(UnityAction<float> action, IngameUIType value)
        {
            switch (value)
            {
                case IngameUIType.Hp:
                    playerHpValueChanged.RemoveListener(action);
                    break;
                case IngameUIType.Mp:
                    playerMpValueChanged.RemoveListener(action);
                    break;
                case IngameUIType.Exp:
                    playerExpValueChanged.RemoveListener(action);
                    break;
                case IngameUIType.Level:
                    playerLevelValueChanged.RemoveListener(action);
                    break;
            }
        }

        public void AddEventToAddItemSystemMessage(UnityAction<string> action)
        {
            AddItemSystemMessage.AddListener(action);
        }

        public void SetBoCharacterFullHp()
        {
            GameManager.User.boCharacter.currentHp = GameManager.User.boCharacter.maxHp;
            GameManager.User.boCharacter.currentMana = GameManager.User.boCharacter.maxMana;
        }

        public Vector3 GetCharacterLastPosition()
        {
            return character.transform.position;
        }

        public void RespawnCharacter()
        {
            var position = GameManager.User.boStage.sdStage.warpPosition[0];
            character.rig.velocity = Vector3.zero;
            character.transform.position = position.GetVector3() + new Vector3(0, 0.5f, 0);
            character.OnHit(fallDamage);
        }
    }
}