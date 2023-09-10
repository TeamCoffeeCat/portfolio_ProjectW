using ProjectW.SD;
using System;
using System.Linq;
using UnityEngine;

namespace ProjectW.DB
{
    [Serializable]
    public class BoStage
    {
        /// <summary>
        /// 플레이어가 마지막으로 위치한 좌표
        /// </summary>
        public Vector3 lastPos;

        /// <summary>
        /// 플레이어가 마지막으로 위치한 스테이지의 기획 데이터
        /// </summary>
        public SDStage sdStage;
    
        public BoStage(DtoStage dtoStage)
        {
            sdStage = GameManager.SD.sdStages.Where(_ => _.index == dtoStage.index).SingleOrDefault();

            lastPos = new Vector3(dtoStage.posX, dtoStage.posY, dtoStage.posZ);
        }
    }
}
