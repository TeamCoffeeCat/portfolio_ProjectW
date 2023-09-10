using ProjectW.Network;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Dummy
{
    /// <summary>
    /// ���̼����� ������ ������ Ŭ����
    /// ���̼������� ����� db�� ���´�.
    /// </summary>
    public class DummyServer : Singleton<DummyServer>
    {
        /// <summary>
        /// ���̼������� ���� ���������� (���� DB)
        /// </summary>
        public UserData userData;

        /// <summary>
        /// ���̼����� ��� ����� ���� ���
        /// </summary>
        public INetworkClient dummyModule;

        public void Initialize()
        {
            dummyModule = new ServerModuleDummy(this);
        }
    }
}