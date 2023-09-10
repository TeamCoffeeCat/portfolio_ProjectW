using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    public class SpawnArea : MonoBehaviour
    {
        public float radius;
        public int monsterCount;
        public bool canRespawn = true;

        public void CheckCanRespawn()
        {
            var monsters = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Monster"));

            if (monsters.Length == 0)
            {
                canRespawn = true;
                return;
            }

            canRespawn = false;
        }
    }
}