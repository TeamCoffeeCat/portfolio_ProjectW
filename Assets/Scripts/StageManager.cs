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
    /// 스테이지 관련 기능들을 제어할 클래스
    /// 주로 스테이지 전환 시 처리작업 (리소스 로드 및 인스턴스 생성)
    /// </summary>
    public class StageManager : Singleton<StageManager>
    {
        /// <summary>
        /// 스테이지 전환 시, 스테이지 전환에 필요한 모든 작업이 완료된 상태인지를 나타내는 필드
        /// </summary>
        private bool isReady;

        /// <summary>
        /// 현재 스테이지의 인스턴스
        /// </summary>
        private GameObject currentStage;

        private GameObject character;

        /// <summary>
        /// 스테이지 전환 시 필요한 리소스를 불러오고 인스턴스 생성 및 데이터 바인딩 작업
        ///  -> 이 메서드를 호출하는 시점은 로딩 씬이 활성화 되어있는 상태
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeStage(SDStage sdStage, SceneType sceneType)
        {
            isReady = false;

            // 외부(서버)에서 새로 불러올 스테이지 정보를 이미 받은 상태
            // 그리고 해당 데이터는 게임매니저의 bouser 필드에 존재함
            // 따라서 새로 로드 할 스테이지에 대한 기획 정보를 불러옴
            // sdStage = GameManager.User.boStage.sdStage;

            var resourceManager = ResourceManager.Instance;

            // 현재 스테이지 객체가 존재한다면
            if (!currentStage)
                // 새로운 스테이지 객체를 생성할 것이므로 파괴
                Destroy(currentStage);

            // 새로운 스테이지 객체를 생성
            //  -> 문제가 있음, 해당 객체를 생성하는 시점은 로딩 씬이 활성화 되어 있고
            //     변경하고자 하는 씬은 비활성화 되어있는 상태, 이 때 객체를 생성 시 생성되는 객체는
            //     활성화된 씬에 종속된다.
            //     따라서 최종적으로 인게임 씬으로 전환되었을 때 스테이지가 보이지 않음
            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));

            // 몬스터를 스폰할 맵이라면 NavMesh 필요
            if (currentStage.TryGetComponent(out NavMeshSurface surface))
            {
                surface.RemoveData();
                surface.BuildNavMesh();
            }

            // 위의 문제를 해결하고자 생성한 객체를 로딩 씬에서 변경하고자 하는 씬으로 이동시킨다.
            SceneManager.MoveGameObjectToScene(currentStage, SceneManager.GetSceneByName(sceneType.ToString()));
            GameManager.Instance.currenScene = sceneType;

            yield return null;
        }

        /// <summary>
        /// 위의 ChangeStage 메서드가 씬 전환 도중에 실행되는 작업이라면
        /// OnChangeStageComplete은 씬 전환이 완료된 후에 실행될 작업
        /// ex) 캐릭터, 몬스터 스폰 등
        /// </summary>
        public void OnChangeStageComplete()
        {
            PoolManagerLight.Instance.Initialize();
            SpawnCharacter();
            SetCamera();
            InitUI();
        }

        /// <summary>
        /// 플레이어의 캐릭터 생성 또는 스테이지 이동 시 플레이어 위치 설정
        /// </summary>
        private void SpawnCharacter()
        {
            var boCharacter = GameManager.User.boCharacter;

            // 캐릭터 인스턴스 생성
            character = Instantiate(ResourceManager.Instance.LoadObject(boCharacter.sdCharacter.resourcePath));

            // 유저의 캐릭터가 이전에 위치했던 좌표로 이동
            character.transform.position = GameManager.User.boStage.lastPos;

            // 캐릭터 객체가 갖는 캐릭터 컴포넌트에 접근하여 초기화 시켜준다.
            //  -> 초기화 시 캐릭터 데이터를 전달
            //  -> 워프 시 GameManager의 bo데이터만을 전달(SD데이터로 초기화X)
            var characterComp = character.GetComponent<Character>();
            characterComp.Initialize(boCharacter);

            if (GameManager.Instance.isNewGame)
            {
                characterComp.InitStat();
                GameManager.Instance.isNewGame = false;
            }

            // IngameManager에 Character 추가
            IngameManager.Instance.Init(characterComp);

            // 캐릭터 컨트롤러 초기화
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