using UnityEngine;
using ProjectW.Define;
using ProjectW.Resource;
using ProjectW.Dummy;
using ProjectW.Network;
using System.Collections;

namespace ProjectW
{
    /// <summary>
    /// 타이틀 씬에서 게임 시작 전에 필요한 전반적인 초기화 및
    /// 데이터 로드를 수행할 클래스
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        private bool PressedButton;

        /// <summary>
        /// 현재 페이즈의 완료 상태
        /// </summary>
        private bool loadComplete;

        /// <summary>
        /// 외부에서 loadComplete에 접근하기 위한 프로퍼티
        /// 추가로 현재 페이즈 완료 시 조건에 따라 다음 페이즈로 변경
        /// </summary>
        public bool LoadComplete
        {
            get { return loadComplete; }
            set
            {
                loadComplete = value;

                // 현재 페이즈가 완료되었고, 모든 페이즈가 완료되지 않았다면
                if (loadComplete && !AllLoaded)
                    // 다음 페이즈로 변경
                    NextPhase();
            }
        }

        /// <summary>
        /// 모든 페이즈의 완료 상태
        /// </summary>
        public bool AllLoaded { get; private set; }

        /// <summary>
        /// 현재 페이즈를 나타냄
        /// </summary>
        private IntroPhase introPhase = IntroPhase.Start;

        /// <summary>
        /// 타이틀 컨트롤러 초기화
        /// </summary>
        public void Initialize()
        {
            OnPhase(introPhase);
        }

        /// <summary>
        /// 현재 페이즈에 대한 로직 실행
        /// </summary>
        /// <param name="phase">진행시키고자 하는 현재 페이즈</param>
        private void OnPhase(IntroPhase phase)
        {
            switch (phase)
            {
                case IntroPhase.Start:
                    break;
                case IntroPhase.AppSetting:
                    GameManager.Instance.OnAppSetting();
                    break;
                case IntroPhase.Server:
                    DummyServer.Instance.Initialize();
                    ServerManager.Instance.Initialize();
                    break;
                case IntroPhase.StaticData:
                    GameManager.SD.Initialize();
                    break;
                case IntroPhase.UserData:
                    // 게임 시작시 필요한 유저 데이터를 서버에 요청해서 받아옴
                    new LoginHandler().Connect();
                    break;
                case IntroPhase.Resource:
                    ResourceManager.Instance.Initialize();
                    break;
                case IntroPhase.UI:
                    break;
                case IntroPhase.Complete:
                    var stageManage = StageManager.Instance;
                    GameManager.Instance.LoadScene(SceneType.StartingVillage,
                        StageManager.Instance.ChangeStage(GameManager.User.boStage.sdStage, SceneType.StartingVillage),
                        StageManager.Instance.OnChangeStageComplete);

                    AllLoaded = true;
                    // StartCoroutine(StartAsync());
                    break;
            }

            LoadComplete = true;
        }

        /// <summary>
        /// 페이즈를 다음 페이즈로 변경
        /// </summary>
        private void NextPhase()
        {
            loadComplete = false;
            OnPhase(++introPhase);
        }

        IEnumerator StartAsync()
        {
            yield return new WaitUntil(() => PressedButton);

            GameManager.Instance.LoadScene
                (SceneType.StartingVillage,
                StageManager.Instance.ChangeStage(GameManager.User.boStage.sdStage, SceneType.StartingVillage),
                StageManager.Instance.OnChangeStageComplete);

        }
    }
}
