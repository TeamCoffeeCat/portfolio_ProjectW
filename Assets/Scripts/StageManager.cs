using ProjectW.Define;
using ProjectW.Object;
using ProjectW.Resource;
using ProjectW.Util;
using ProjectW.SD;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using CoffeeCat.Simplify;

namespace ProjectW
{
    /// <summary>
    /// �������� ���� ��ɵ��� ������ Ŭ����
    /// �ַ� �������� ��ȯ �� ó���۾� (���ҽ� �ε� �� �ν��Ͻ� ����)
    /// </summary>
    public class StageManager : Singleton<StageManager>
    {
        /// <summary>
        /// �������� ��ȯ ��, �������� ��ȯ�� �ʿ��� ��� �۾��� �Ϸ�� ���������� ��Ÿ���� �ʵ�
        /// </summary>
        private bool isReady;

        /// <summary>
        /// ���� ���������� �ν��Ͻ�
        /// </summary>
        private GameObject currentStage;

        private GameObject character;

        /// <summary>
        /// �������� ��ȯ �� �ʿ��� ���ҽ��� �ҷ����� �ν��Ͻ� ���� �� ������ ���ε� �۾�
        ///  -> �� �޼��带 ȣ���ϴ� ������ �ε� ���� Ȱ��ȭ �Ǿ��ִ� ����
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeStage(SDStage sdStage, SceneType sceneType)
        {
            isReady = false;

            // �ܺ�(����)���� ���� �ҷ��� �������� ������ �̹� ���� ����
            // �׸��� �ش� �����ʹ� ���ӸŴ����� bouser �ʵ忡 ������
            // ���� ���� �ε� �� ���������� ���� ��ȹ ������ �ҷ���
            // sdStage = GameManager.User.boStage.sdStage;

            var resourceManager = ResourceManager.Instance;

            // ���� �������� ��ü�� �����Ѵٸ�
            if (!currentStage)
                // ���ο� �������� ��ü�� ������ ���̹Ƿ� �ı�
                Destroy(currentStage);

            // ���ο� �������� ��ü�� ����
            //  -> ������ ����, �ش� ��ü�� �����ϴ� ������ �ε� ���� Ȱ��ȭ �Ǿ� �ְ�
            //     �����ϰ��� �ϴ� ���� ��Ȱ��ȭ �Ǿ��ִ� ����, �� �� ��ü�� ���� �� �����Ǵ� ��ü��
            //     Ȱ��ȭ�� ���� ���ӵȴ�.
            //     ���� ���������� �ΰ��� ������ ��ȯ�Ǿ��� �� ���������� ������ ����
            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));

            // ���͸� ������ ���̶�� NavMesh �ʿ�
            if (currentStage.TryGetComponent(out NavMeshSurface surface))
            {
                surface.RemoveData();
                surface.BuildNavMesh();
            }

            // ���� ������ �ذ��ϰ��� ������ ��ü�� �ε� ������ �����ϰ��� �ϴ� ������ �̵���Ų��.
            SceneManager.MoveGameObjectToScene(currentStage, SceneManager.GetSceneByName(sceneType.ToString()));
            GameManager.Instance.currenScene = sceneType;

            yield return null;
        }

        /// <summary>
        /// ���� ChangeStage �޼��尡 �� ��ȯ ���߿� ����Ǵ� �۾��̶��
        /// OnChangeStageComplete�� �� ��ȯ�� �Ϸ�� �Ŀ� ����� �۾�
        /// ex) ĳ����, ���� ���� ��
        /// </summary>
        public void OnChangeStageComplete()
        {
            PoolManagerLight.Instance.Initialize();
            SpawnCharacter();
            SetCamera();
            InitUI();
        }

        /// <summary>
        /// �÷��̾��� ĳ���� ���� �Ǵ� �������� �̵� �� �÷��̾� ��ġ ����
        /// </summary>
        private void SpawnCharacter()
        {
            var boCharacter = GameManager.User.boCharacter;

            // ĳ���� �ν��Ͻ� ����
            character = Instantiate(ResourceManager.Instance.LoadObject(boCharacter.sdCharacter.resourcePath));

            // ������ ĳ���Ͱ� ������ ��ġ�ߴ� ��ǥ�� �̵�
            character.transform.position = GameManager.User.boStage.lastPos;

            // ĳ���� ��ü�� ���� ĳ���� ������Ʈ�� �����Ͽ� �ʱ�ȭ �����ش�.
            //  -> �ʱ�ȭ �� ĳ���� �����͸� ����
            //  -> ���� �� GameManager�� bo�����͸��� ����(SD�����ͷ� �ʱ�ȭX)
            var characterComp = character.GetComponent<Character>();
            characterComp.Initialize(boCharacter);

            if (GameManager.Instance.isNewGame)
            {
                characterComp.InitStat();
                GameManager.Instance.isNewGame = false;
            }

            // IngameManager�� Character �߰�
            IngameManager.Instance.Init(characterComp);

            // ĳ���� ��Ʈ�ѷ� �ʱ�ȭ
            var controller = new GameObject(name = "PlayerController").AddComponent<PlayerController>();
            controller.Initialize(characterComp);
        }

        public void WarpStage(SceneType sceneType, int warpIndex)
        {
            var stage = GameManager.SD.sdStages.FirstOrDefault(_ => _.name == sceneType.ToString());
            GameManager.User.boStage.sdStage = stage;
            GameManager.Instance.LoadScene(sceneType, ChangeStage(stage, sceneType), OnWarpStageComplete);
            return;

            void OnWarpStageComplete()
            {
                PoolManagerLight.Instance.Initialize();
                SpawnCharacter();
                InitSpawner();
                InitUI();

                character.transform.position = stage.warpPosition[warpIndex].GetVector3();
                SetCamera();
            }
        }

        private void SetCamera()
        {
            var cameraController = UnityEngine.Camera.main.GetComponent<CameraController>();
            cameraController.SetTarget(character.transform);
        }

        private void InitUI()
        {
            UIPresenter.Instance.InitUI(character.GetComponent<Character>());
        }

        private void InitSpawner()
        {
            var spawner = FindObjectOfType<MonsterSpawner>();

            if (spawner)
                spawner.Init();
        }
    }
}