using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Network
{
    /// <summary>
    /// Ŭ���̾�Ʈ �� ��ü���� ���� ����� �����ϴ� �Ŵ���
    /// ��Ȳ�� ���� ���� ����� �����Ͽ� ����� ó���Ѵ�.
    /// ���⼭ ���ϴ� ��Ȳ�� ���� ���� ����̶�?
    ///  ex) ���̺� ������� ���̺꼭�� ���, ���̼������ ���̼��� ��� ����
    /// </summary>
    public class ServerManager : Singleton<ServerManager>
    {
        /// <summary>
        /// ��Ȳ�� �´� ��������� ���� �ʵ�
        /// </summary>
        private INetworkClient netClient;
        public static INetworkClient Server => Instance.netClient;

        public void Initialize()
        {
            // ���� ��� ���丮�� ���� ���� ��Ȳ�� �´� ��������� ��ȯ�޴´�.
            netClient = ServerModuleFactory.NewNetworkClientModule();
        }
    }
}