using Newtonsoft.Json;
using ProjectW.Object;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectW.SD
{
    /// <summary>
    /// ��� ��ȹ �����͸� ������ Ŭ����
    /// ��ȹ �����͸� �ε��ϰ� ��� �ֱ⸸ �� ���̹Ƿ�
    /// ��븦 ��� ���� �ʿ䰡 ����
    /// </summary>
    // ��븦 ���� �ʴ� Ŭ������ �ν����Ϳ� �����Ű�� ���� ����ȭ
    [Serializable]
    public class StaticDataModule
    {
        public List<SDCharacter> sdCharacters;
        public List<SDStage> sdStages;
        public List<SDMonster> sdMonster;
        public List<SDItem> sdItems;
        public List<SDGrowthStat> sdGrowthStat;

        public void Initialize()
        {
            // Json���� ��ȯ�� ��ȹ�����͸� �о���� ���� Loader
            var loader = new StaticDataLoader();
            
            loader.Load(out sdCharacters);
            loader.Load(out sdStages);
            loader.Load(out sdMonster);
            loader.Load(out sdItems);
            loader.Load(out sdGrowthStat);

            // Ÿ�Ժ��� �������� �з�
            loader.SortSdItems(sdItems);
        }

        private class StaticDataLoader
        {
            private string path;

            public StaticDataLoader()
            {
#if UNITY_EDITOR
                // Application.dataPath : ������� ������Ʈ ���� ����(Asset)
                path = $"{Application.dataPath}/StaticData/Json";
#else
                path = string.Empty;
#endif
            }

            // ��ȹ������ json�� �о�� TŸ�� ������ ����Ʈ�� �Ľ�
            public void Load<T>(out List<T> data) where T : StaticData
            {
                // ��� ��ȹ�����ʹ� SD ��� ���ξ�� ����
                var fileName = typeof(T).Name.Remove(0, "SD".Length);
                string jsonStr = string.Empty;
                
#if UNITY_EDITOR
                jsonStr = File.ReadAllText($"{path}/{fileName}.json");
#else
                jsonStr = Resources.Load<TextAsset>($"Json/{fileName}").text;
#endif
                data = JsonConvert.DeserializeObject<List<T>>(jsonStr);
            }

            public void SortSdItems(List<SDItem> sdItems)
            {
                for (int i = 0; i < sdItems.Count; i++)
                {
                    switch (sdItems[i].subType)
                    {
                        case Define.Item.SubType.HpPotion:
                            sdItems[i] = new HpPotion(sdItems[i]);
                            break;
                        case Define.Item.SubType.MpPotion:
                            sdItems[i] = new MpPotion(sdItems[i]); 
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}