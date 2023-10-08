using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using ProjectW.Dummy;
using ProjectW.Util;

namespace ProjectW.DB
{
    public class DataManager : Singleton<DataManager>
    {
        private string path;

        private void Start()
        {
            path = Application.persistentDataPath;
        }

        public static void CreateNewCharacter(string nickname)
        {
            var userData = new UserData(nickname);
            DummyServer.Instance.userData = userData;
            GameManager.Instance.isNewGame = true;
            GameManager.Instance.InitTitleController();
        }

        public void LoadGame(UserData userData)
        {
            DummyServer.Instance.userData = userData;
            GameManager.Instance.InitTitleController();
        }

        public void SaveUserData(int dataNum)
        {
            SetUserData(GameManager.User);

            string saveData = JsonConvert.SerializeObject(DummyServer.Instance.userData);
            File.WriteAllText($"{path}/SaveData_{dataNum}.json", saveData);
        }

        private void SetUserData(BoUser boUser)
        {
            var userData = DummyServer.Instance.userData;
            var lastPos = IngameManager.Instance.GetCharacterLastPosition();

            userData.dtoAccount.gold = boUser.boAccount.gold;
            userData.dtoCharacter.level = boUser.boCharacter.level;
            userData.dtoCharacter.maxExp = boUser.boCharacter.maxExp;
            userData.dtoCharacter.currentExp = boUser.boCharacter.currentExp;
            userData.dtoCharacter.maxHp = boUser.boCharacter.maxHp;
            userData.dtoCharacter.currentHp = boUser.boCharacter.currentHp;
            userData.dtoCharacter.maxMana = boUser.boCharacter.maxMana;
            userData.dtoCharacter.currentMana = boUser.boCharacter.currentMana;
            userData.dtoCharacter.atk = boUser.boCharacter.atk;
            userData.dtoCharacter.def = boUser.boCharacter.def;
            userData.dtoStage.index = boUser.boStage.sdStage.index;
            userData.dtoStage.posX = lastPos.x;
            userData.dtoStage.posY = lastPos.y;
            userData.dtoStage.posZ = lastPos.z;

            foreach (BoItem boItem in boUser.boInventory.BoItems)
            {
                var dtoItem = new DtoItem();
                dtoItem.index = boItem.SdItem.index;
                dtoItem.itemCount = boItem.ItemCount;
                userData.dtoInventory.DtoItems.Add(dtoItem);
            }
        }

        public bool IsExistSaveData(int dataNum)
        {
            return File.Exists($"{path}/SaveData_{dataNum}.json");
        }

        public UserData LoadUserData(int dataNum)
        {
            var saveData = File.ReadAllText($"{path}/SaveData_{dataNum}.json");
            return JsonConvert.DeserializeObject<UserData>(saveData);
        }
    }
}