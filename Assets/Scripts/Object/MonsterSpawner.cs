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
            // �ش� ������ ���� �� ���� ����ŭ �ݺ�
            for (int i = 0; i < spawnArea.monsterCount; i++)
            {
                // ���� ������ �߽����� ���Ƿ� ������ ������ŭ ��ü�� �����Ͽ� �� �ȿ��� ������ ��ǥ�� ����
                var spawnPos = spawnArea.transform.position + Random.insideUnitSphere * spawnArea.radius;
                spawnPos.y = posY;
                // Pool���� ���� Ȱ��ȭ �� ���� ����Ʈ Ȱ��ȭ
                var monster = PoolManagerLight.Instance.SpawnToPool("Monster_Cat", spawnPos, Quaternion.identity);
                monster.GetComponent<Monster>().SetStats();
                PoolManagerLight.Instance.SpawnEffect("Monster_Spawn", spawnPos, Quaternion.identity, 1.5f);
            }
        }
    }
}