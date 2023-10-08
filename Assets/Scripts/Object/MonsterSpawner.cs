using CoffeeCat.Simplify;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ProjectW.Object
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    public class MonsterSpawner : MonoBehaviour
    {
        private SpawnArea[] spawnAreas;
        private WaitForSeconds waitForSeconds;
        public float posY;

        public void Init()
        {
            spawnAreas = new SpawnArea[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                spawnAreas[i] = transform.GetChild(i).GetComponent<SpawnArea>();
            }

            StartCoroutine(AutoSpawn(20f));
        }

        private IEnumerator AutoSpawn(float intervalTime)
        {
            waitForSeconds = new WaitForSeconds(intervalTime);

            while (true)
            {
                foreach (SpawnArea spawnArea in spawnAreas)
                {
                    if (spawnArea.CanRespawn())
                    {
                        SpawnMonster(spawnArea);
                    }
                }
                yield return waitForSeconds;
            }
        }

        private void SpawnMonster(SpawnArea spawnArea)
        {
            // 해당 구역의 스폰 할 몬스터 수만큼 반복
            for (int i = 0; i < spawnArea.monsterCount; i++)
            {
                // 스폰 구역을 중식으로 임의로 지정한 영역만큼 구체를 지정하여 그 안에서 랜덤한 좌표를 생성
                var spawnPos = spawnArea.transform.position + Random.insideUnitSphere * spawnArea.radius;
                spawnPos.y = posY;
                // Pool에서 몬스터 활성화 및 스폰 이펙트 활성화
                var monster = PoolManagerLight.Instance.SpawnToPool("Monster_Cat", spawnPos, Quaternion.identity);
                monster.GetComponent<Monster>().SetStats();
                PoolManagerLight.Instance.SpawnEffect("Monster_Spawn", spawnPos, Quaternion.identity, 1.5f);
            }
        }
    }
}