using System;
using ProjectW.Network;

namespace ProjectW.DB
{
    /// <summary>
    /// ������ ���������� ��Ÿ���� �����ͼ�
    ///  -> ������ Ŭ���̾�Ʈ ���� ������ ��Ž� ���� ���������� �ش� Ŭ���� ���·� �ٷ��.
    /// </summary>
    [Serializable]
    public class DtoAccount : DtoBase
    {
        /// <summary>
        /// ���� �г���
        /// </summary>
        public string nickname;

        /// <summary>
        /// ������ ���
        /// </summary>
        public int gold;

        public DtoAccount(string nickname)
        {
            this.nickname = nickname;
            gold = 0;
        }
    }
}