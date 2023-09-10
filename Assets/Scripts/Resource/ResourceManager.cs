using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Resource
{
    /// <summary>
    /// ��Ÿ��(����ð�)�� �ʿ��� ���ҽ��� �ҷ����� ����� ����� Ŭ����
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        public void Initialize()
        {

        }

        /// <summary>
        /// Assets/Resources ���� ���� �������� �ҷ��� ��ȯ�ϴ� ���
        /// </summary>
        /// <param name="path">Resources ���� �� �ҷ��� ���� ���</param>
        /// <returns>�ҷ��� ���ӿ�����Ʈ</returns>
        public GameObject LoadObject(string path)
        {
            // Resources.Load -> Assets ���� �� Resources ��� �̸��� ������ �����Ѵٸ�
            // �ش� ��κ��� path�� ����, �ش� ��ο� ������ GameObject ���·� �θ� �� �ִٸ� �ҷ���
            return Resources.Load<GameObject>(path);
        }

        /// <summary>
        /// ������Ʈ Ǯ�� ����� ��ü�� �������� �ε� ��, ������Ʈ Ǯ �Ŵ����� �̿��Ͽ� Ǯ�� ����ϴ� ���
        /// </summary>
        /// <typeparam name="T">�ε��ϰ��� �ϴ� Ÿ��</typeparam>
        /// <param name="path">������ ���</param>
        /// <param name="poolCount">Ǯ ��Ͻ�, �ʱ� �ν��Ͻ� ��</param>
        /// <param name="loadComplete">�ε� �� ��� �Ϸ� �� �����ų �̺�Ʈ</param>
        public void LoadPoolableObject<T>(string path, int poolCount = 1, Action loadComplete = null) where T : MonoBehaviour, IPoolableObject
        {
            // �������� �ε��Ѵ�.
            var obj = LoadObject(path);
            // �������� ���� �ִ� TŸ�� ������Ʈ ������ �����´�.
            var tComponent = obj.GetComponent<T>();

            // tŸ���� Ǯ�� ���
            ObjectPoolManager.Instance.RegistPool<T>(tComponent, poolCount);

            // �� �۾��� ��� ���� ��, �����ų ����� �ִٸ� ����
            loadComplete?.Invoke();

        }
    }
}