using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Object
{
    public class SpawnArea : MonoBehaviour
    {
        public float radius;
        public int monsterCount;

        public bool CanRespawn()
        {
            var hitMon = new Collider[monsterCount];
            var monsters = Physics.OverlapSphereNonAlloc(transform.position, radius, hitMon, 1 << LayerMask.NameToLayer("Monster"));

            return monsters <= 0;
        }
    }
}