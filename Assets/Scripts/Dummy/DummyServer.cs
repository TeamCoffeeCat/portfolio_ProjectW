using ProjectW.Network;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Dummy
{
    // ���̼����� ������ ������ Ŭ����
    // ���̼������� ����� db�� ���´�.
    public class DummyServer : Singleton<DummyServer>
    {
        // ���̼������� ���� ���������� (���� DB)
        public UserData userData;

        // ���̼����� ��� ����� ���� ���
        public INetworkClient dummyModule;

        public void Initialize()
        {
            dummyModule = new ServerModuleDummy(this);
        }
    }
}