using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.SD
{
    [Serializable]
    public class SDGrowthStat : StaticData
    {
        public string name;
        public float maxHp;
        public float maxMana;
        public float maxExp;
        public float atk;
        public float def;
    }
}