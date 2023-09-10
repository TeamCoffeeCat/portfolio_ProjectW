using CoffeeCat.Simplify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoffeeCat;

namespace ProjectW.Object
{
    public class MonsterSpawner : MonoBehaviour
    {
        private SpawnArea[] spawnAreas;
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
            while (true)
            {
                foreach (SpawnArea spawnArea in spawnAreas)
                {
                    spawnArea.CheckCanRespawn();
                    SpawnMonster(spawnArea);
                }

                yield return new WaitForSeconds(intervalTime);
            }
        }

        public void SpawnMonster(SpawnArea spawnArea)
        {
            if (!spawnArea.canRespawn)
                return;

            for (int i = 0; i < spawnArea.monsterCount; i++)
            {
                var spawnPos = spawnArea.transform.position + Random.insideUnitSphere * spawnArea.radius;
                spawnPos.y = posY;
                var monster = PoolManagerLight.Instance.SpawnToPool("Monster_Cat", spawnPos, Quaternion.identity);
                monster.GetComponent<Monster>().SetStats();
                PoolManagerLight.Instance.SpawnEffect("Monster_Spawn", spawnPos, Quaternion.identity, 1.5f);
            }

            spawnArea.canRespawn = false;
        }

        // TO DO
        // 몬스터 체력바
    }
}