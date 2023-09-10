using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectW.Util;
using ProjectW.SD;
using ProjectW.Define;
using System;
using UnityEngine.SceneManagement;
using ProjectW.DB;

namespace ProjectW
{
    /// <summary>
    /// ���ӿ� ����ϴ� ��� �����͸� �����ϴ� Ŭ����
    /// �߰��� ������ �� ���� ��� ���� ū �帧�� ������
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// ���̼����� ����� �������� ���� �÷���
        /// </summary>
        public bool useDummyServer;

        /// <summary>
        /// �� ��ȯ ��, ���� �ε� ���¸� ��Ÿ���� �ʵ�
        /// 0~1 �� ���
        /// </summary>
        public float loadState;

        public bool isNewGame;

        public SceneType currenScene;

        // ���� ������
        [SerializeField]
        private BoUser boUser = new BoUser();
        public static BoUser User => Instance.boUser;

        // ��ȹ ������
        [SerializeField]
        private StaticDataModule sd = new StaticDataModule();
        public static StaticDataModule SD => Instance.sd;

        protected override void Awake()
        {
            base.Awake();

            currenScene = SceneType.Title;
        }

        public void InitTitleController()
        {
            // Ÿ��Ʋ ������ ������ �ε带 �����ϱ� ����
            // Ÿ��Ʋ ��Ʈ�ѷ� ������ ã�´�.
            var titleController = FindObjectOfType<TitleController>();
            // ������ �����Ѵٸ�? �ʱ�ȭ
            titleController?.Initialize();
        }

        /// <summary>
        /// �� �⺻ ����
        /// </summary>
        public void OnAppSetting()
        {
            // ��������ȭ ����
            QualitySettings.vSyncCount = 0;
            // ���� ������ 60 ����
            Application.targetFrameRate = 60;
            // �� ���� �� ��ð� ��� �ÿ��� ȭ���� ������ �ʵ���
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    
        /// <summary>
        /// ���� �񵿱�� �ε��ϴ� ���
        /// �ٸ� �� ���� ��ȯ�� ��� (ex Title -> Ingame)
        /// </summary>
        /// <param name="type">�ε��� ���� �̸��� ���� ������</param>
        /// <param name="loadProgress">�� ��ȯ �� �ε� ������ �̸� ó���� �۾�</param>
        /// <param name="loadComplete">�� ��ȯ �Ϸ� �� ������ ���</param>
        public void LoadScene(SceneType type, IEnumerator loadProgress = null, Action loadComplete = null)
        {
            StartCoroutine(WaitForLoad());

            // ���� ��ȯ�� ��, ex) Title -> Ingame ��ȯ �� �� ���� ��ȯ�ϴ� ���� �ƴ϶�
            // �߰��� �ε� ���� �̿�, ���������� Title -> Loading -> Ingame

            // �ڷ�ƾ -> ����Ƽ���� Ư�� �۾��� �񵿱�� ������ �� �ְ� ���ִ� ���
            //           ���� �񵿱�� �ƴ�, ���� Ÿ���� �ٸ��� �Ͽ� �񵿱�ó�� ���̰� �ϴ� ��
            
            // LoadScene �޼��忡�� ��밡���� �����Լ� ����
            IEnumerator WaitForLoad()
            {
                // �ε� ���� ���¸� 0���� �ʱ�ȭ
                loadState = 0;

                // �񵿱�� ���� ������ �ε� ������ ��ȯ
                //  -> �� ��ȯ ��, ȭ���� ������ �ʰ� �ϱ� ���ؼ�
                yield return SceneManager.LoadSceneAsync(SceneType.Loading.ToString());

                // �ε� ������ ��ȯ �Ϸ� �Ŀ� �Ʒ� ������ ����

                // ���� �����ϰ��� �ϴ� ���� �߰� (���� ���ӿ� ���� 2���� �� ����)
                //  -> LoadScene�� �� ���� ��, 2��° �Ķ���͸� ������� �ʴ´ٸ� �⺻ ���� �����
                //     �⺻ ���� ���� ���� ��Ȱ��ȭ�ϰ� �����ϰ��� �ϴ� ���� Ȱ��ȭ
                //     ������, ���� �ϰ��� �ϴ� �۾��� ���� ���� �״�� �ΰ� ���ο� ���� �߰��ϴ� �۾�
                //     �� ��, �� �ε� ����� Additive�� �����ϸ� ��
                var asyncOper = SceneManager.LoadSceneAsync(type.ToString(), LoadSceneMode.Additive);

                // ��������� ���� ���ӿ��� 2���� ���� Ȱ��ȭ�� ���� (�ε� ��, �����ϰ��� �ϴ� ��)
                // ���� ��ġ �ʰ� 2���� ���� ��ü���� ��� �����ǹǷ�, ���� ���� �ʿ����� ����
                // �����ϰ��� �ϴ� ���� ��Ȱ��ȭ�Ѵ�.
                //  -> ���� �״�� ���̶�Ű �� �����ϴµ� ��Ȱ��ȭ�� ��Ŵ

                // ��Ȱ��ȭ ��Ű�� ��? �񵿱�� �� �ε� �� LoadSceneAsync �޼��尡 AsyncOper ��ü�� ��ȯ��
                // AsyncOper ��ü�� ���� �񵿱�� �θ��� �ִ� ���� �ε� ����, Ȱ��ȭ ���� ���� �� �� ����
                asyncOper.allowSceneActivation = false;

                // �����ϰ��� �ϴ� ���� �ʿ��� �۾��� �����Ѵٸ� ����
                if (loadProgress != null)
                {
                    // �ش� �۾��� �Ϸ�� ������ ���
                    yield return StartCoroutine(loadProgress);
                }

                // ���� �۾��� �Ϸ�� �Ŀ� ������ �����
                //  -> �����ϰ��� �ϴ� ���� �ʿ��� �۾��� �� ������ ��� �Ϸ�ƴٴ� ��

                // �ε��ٿ� ���� ���¸� �����Ͽ� �������� �ε� ���¸� �˸�
                // �߰��� �����ϰ��� �ϴ� ���� �ε尡 �Ϸ�� ���������� Ȯ���ؾ� ��
                //  -> �񵿱�� �ε��Ͽ���, �ε� �� yield return ���� �ʾұ� ������ ��� ������ �Ϸ�� �� �� �� ����

                // �񵿱�� �ε��� ���� �ε尡 �Ϸ���� �ʾҴٸ� Ư�� �۾��� �ݺ�
                while (!asyncOper.isDone)
                {
                    // loadState ���� �̿��� �������� ���¸� �˸�

                    if (loadState >= 0.9f)
                    {
                        // 90�ۼ�Ʈ �̻� �Ϸ�ƴٸ� ������ 100�ۼ�Ʈ�� ����
                        //  ����? asyncOper�� progress�� ���� ��Ȯ�ϰ� 1�� ������ ����
                        loadState = 1f;

                        // �ε��ٰ� ���������� ���� ���� �������� �����ֱ� ���� 1�� ���� ���
                        yield return new WaitForSeconds(1f);

                        // �����ϰ��� �ϴ� ���� �ٽ� Ȱ��ȭ
                        // (isDone�� ���� Ȱ�� ���°� �ƴ϶��, �ε尡 �Ϸ�Ǿ����� true�� ���� ����)
                        asyncOper.allowSceneActivation = true;
                    }
                    else
                    {
                        // asyncOper�� ���� �ε� ���� ���¸� 0���� 1������ ������ ��Ÿ���� ������Ƽ�� ����
                        // �ش� ���� loadState�� ����

                        loadState = asyncOper.progress;
                    }

                    // �ڷ�ƾ ������ �ݺ��� ��� ��, �ݺ��� ���� ������ �� �� ���� ��
                    // ���� ������ ������ �� �ְ� yield return
                    yield return null;
                }

                // �ε� ������ �����ϰ��� �ϴ� ���� �ʿ��� �۾��� ���� ���������Ƿ�
                // �ε����� ��Ȱ��ȭ ��Ŵ
                yield return SceneManager.UnloadSceneAsync(SceneType.Loading.ToString());

                // ��� �۾��� �Ϸ�Ǿ����Ƿ�, �߰������� ������ �۾��� �����Ѵٸ�? ����
                loadComplete?.Invoke(); 
            }
        }
    }
}
