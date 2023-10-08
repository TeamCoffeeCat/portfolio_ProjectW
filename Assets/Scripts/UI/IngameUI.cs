using ProjectW;
using ProjectW.DB;
using ProjectW.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum IngameUIType
{
    Hp,
    Mp,
    Exp,
    Level,
}

public class IngameUI : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI NicknameText;
    public TextMeshProUGUI HpValue;
    public TextMeshProUGUI MpValue;
    public TextMeshProUGUI ExpValue;

    public Image HpBarFill;
    public Image MpBarFill;
    public Image ExpBarFill;

    public GameObject RespawnMessage;
    public Button ConfirmButton;

    public GameObject SystemMenu;
    public Button SaveButton;
    public Button QuitButton;

    public Transform SystemMessageParent;
    private SystemMessage[] SystemMessages;

    private BoCharacter boCharacter;

    public void Init(Character character)
    {
        boCharacter = character.boCharacter;

        // Ingame UI에서 사용 할 플레이어의 스탯 Change 이벤트 추가 및 Invoke
        #region PlayerUIValueChaged
        NicknameText.text = GameManager.User.boAccount.nickname;
        IngameManager.Instance.AddEventToIngameUIValueChanged(OnUpdateHpUI, IngameUIType.Hp);
        IngameManager.Instance.AddEventToIngameUIValueChanged(OnUpdateMpUI, IngameUIType.Mp);
        IngameManager.Instance.AddEventToIngameUIValueChanged(OnUpdateExpUI, IngameUIType.Exp);
        IngameManager.Instance.AddEventToIngameUIValueChanged(OnUpdatePlayerLevelUI, IngameUIType.Level);

        IngameManager.Instance.playerHpValueChanged.Invoke(boCharacter.currentHp);
        IngameManager.Instance.playerMpValueChanged.Invoke(boCharacter.currentMana);
        IngameManager.Instance.playerLevelValueChanged.Invoke(boCharacter.level);
        IngameManager.Instance.playerExpValueChanged.Invoke(boCharacter.currentExp);
        #endregion

        #region SystemMessage
        SystemMessages = new SystemMessage[SystemMessageParent.childCount];
        for (int i = 0; i < SystemMessages.Length; i++)
        {
            SystemMessages[i] = SystemMessageParent.GetChild(i).GetComponent<SystemMessage>();
        }

        IngameManager.Instance.AddEventToAddItemSystemMessage(ShowSystemMessage);
        #endregion

        ConfirmButton.onClick.AddListener(() =>
        {
            IngameManager.Instance.SetBoCharacterFullHp();
            StageManager.Instance.WarpStage(ProjectW.Define.SceneType.StartingVillage, 0);
        });
    }

    public void OnUpdateHpUI(float value)
    {
        HpValue.text = $"{(int)value} / {boCharacter.maxHp}";
        HpBarFill.fillAmount = value / boCharacter.maxHp;
    }

    public void OnUpdateMpUI(float value)
    {
        MpValue.text = $"{(int)value} / {boCharacter.maxMana}";
        MpBarFill.fillAmount = value / boCharacter.maxMana;
    }

    public void OnUpdateExpUI(float value)
    {
        ExpValue.text = $"{(int)value} / {(int)boCharacter.maxExp}";
        ExpBarFill.fillAmount = value / boCharacter.maxExp;
    }

    public void OnUpdatePlayerLevelUI(float value)
    {
        LevelText.text = $"Lv.{boCharacter.level}";
    }

    public void ShowSystemMessage(string value)
    {
        var text = $"아이템 [ {value} ] 획득";
        PickSystemMessage().ShowSystemMessage(text);
    }
    

    private SystemMessage PickSystemMessage()
    {
        foreach (SystemMessage message in SystemMessages)
        {
            if (message.IsReady)
            {
                return message;
            }
        }

        return SystemMessages[0];
    }
}
