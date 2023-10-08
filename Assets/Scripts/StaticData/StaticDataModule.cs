using Newtonsoft.Json;
using ProjectW.Object;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProjectW.SD
{
    /// <summary>
    /// 모든 기획 데이터를 관리할 클래스
    /// 기획 데이터를 로드하고 들고 있기만 할 것이므로
    /// 모노를 상속 받을 필요가 없음
    /// </summary>
    // 모노를 갖지 않는 클래스를 인스펙터에 노출시키기 위해 직렬화
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
            // Json으로 변환된 기획데이터를 읽어오기 위한 Loader
            var loader = new StaticDataLoader();
            
            loader.Load(out sdCharacters);
            loader.Load(out sdStages);
            loader.Load(out sdMonster);
            loader.Load(out sdItems);
            loader.Load(out sdGrowthStat);

            // 타입별로 아이템을 분류
            loader.SortSdItems(sdItems);
        }

        private class StaticDataLoader
        {
            private string path;

            public StaticDataLoader()
            {
#if UNITY_EDITOR
                // Application.dataPath : 사용중인 프로젝트 폴더 내부(Asset)
                path = $"{Application.dataPath}/StaticData/Json";
#else
                path = string.Empty;
#endif
            }

            // 기획데이터 json을 읽어와 T타입 데이터 리스트로 파싱
            public void Load<T>(out List<T> data) where T : StaticData
            {
                // 모든 기획데이터는 SD 라는 접두어로 시작
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