using System;

namespace ProjectW.SD
{
    [Serializable]
    public class SDStage : StaticData
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 해당 스테이지에서 생성될 수 있는 몬스터들의 기획 데이터 상의 인덱스 
        ///  -> 나중에 설명
        /// </summary>
        public int[] genMonsters;
        
        /// <summary>
        /// 나중에 설명
        /// </summary>
        public int[] spawnArea;
        
        /// <summary>
        /// 나중에 설명
        /// </summary>
        public int[] warpStageRef;

        /// <summary>
        /// 워프 포탈 좌표
        /// </summary>
        public JVector3[] warpPosition;

        /// <summary>
        /// 프리팹 경로
        /// </summary>
        public string resourcePath;
    }
}
