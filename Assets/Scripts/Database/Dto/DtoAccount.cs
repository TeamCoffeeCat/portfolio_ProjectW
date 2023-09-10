using System;
using ProjectW.Network;

namespace ProjectW.DB
{
    /// <summary>
    /// 유저의 계정정보를 나타내는 데이터셋
    ///  -> 서버와 클라이언트 간의 데이터 통신시 유저 계정정보를 해당 클래스 형태로 다룬다.
    /// </summary>
    [Serializable]
    public class DtoAccount : DtoBase
    {
        /// <summary>
        /// 유저 닉네임
        /// </summary>
        public string nickname;

        /// <summary>
        /// 보유한 골드
        /// </summary>
        public int gold;

        public DtoAccount(string nickname)
        {
            this.nickname = nickname;
            gold = 0;
        }
    }
}