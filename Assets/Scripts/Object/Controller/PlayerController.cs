using ProjectW.Controller;
using UnityEngine;
using InputAxisEvent = ProjectW.Controller.InputController.InputAxisEvent;
using State = ProjectW.Define.Actor.State;

namespace ProjectW.Object
{
    using Input = Define.Input;

    /// <summary>
    /// 플레이어 캐릭터의 입력처리
    /// 캐릭터 클래스에서 처리 안하는 이유?
    /// 캐릭터와 플레이어 입력을 분리함으로써
    /// 캐릭터 클래스를 더 다양하게 사용할 수 있기 때문에 (ex: 멀티 환경에서의 캐릭터 제어 등)
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private InputController inputController;

        public Character PlayerCharacter { get; private set; }

        private Transform mainCamera;

        /// <summary>
        /// 초기화 시, 플레이어 캐릭터의 참조를 주입받는다.
        /// </summary>
        /// <param name="character"></param>
        public void Initialize(Character character)
        {
            transform.position = character.transform.position;
            mainCamera = Camera.main.transform;

            // 플레이어 캐릭터의 하이라키 상의 부모를 플레이어 컨트롤러로 지정
            character.transform.SetParent(transform);
            // 플레이어 캐릭터의 태그를 Player 태그로 지정
            // (다른 캐릭터가 존재한다고 가정했을 때, 편리하게 플레이어의 캐릭터를 구분하기 위함)
            character.gameObject.layer = LayerMask.NameToLayer("Player");

            // 플레이어 캐릭터 참조 전달
            PlayerCharacter = character;

            // 입력 컨트롤러 객체 생성
            inputController = new InputController();

            // 축 타입 키 등록
            inputController.AddAxis(Input.AxisX, new InputAxisEvent(GetAxisX)); // 캐릭터 좌, 우 이동
            inputController.AddAxis(Input.AxisZ, new InputAxisEvent(GetAxisZ)); // 캐릭터 앞, 뒤 이동
            inputController.AddAxis(Input.MouseX, new InputAxisEvent(CameraRotation)); // 카메라 회전

            // 버튼 타입 키 등록
            // inputController.AddButton(Input.Jump, // 캐릭터 점프
            //     downEvent: new InputButtonEvent(SpaceDown));
            
            // inputController.AddButton(Input.MouseLeft, // 공격
            //     downEvent: new InputButtonEvent(MouseLeft),
            //     pressEvent: new InputButtonEvent(MouseLeft));
        }

        private void FixedUpdate()
        {
            // 플레이어 캐릭터가 세팅되기 전에는 입력할 수 없도록
            if (!PlayerCharacter)
                return;

            // 플레이어가 죽었다면 입력할 수 없도록
            if (PlayerCharacter.stateMachine.state == State.Die)
                return;

            InputUpdate();
            ActiveInventoryUI();
            ActiveSaveDataUI();
        }

        private void InputUpdate()
        {
            // 축 타입의 키 체크
            foreach (var inputAxis in inputController.inputAxes)
            {
                inputAxis.Value.GetAxisValue(UnityEngine.Input.GetAxisRaw(inputAxis.Key));
            }

            // 버튼 타입의 키 체크
            foreach (var inputButton in inputController.inputButtons)
            {
                if (UnityEngine.Input.GetButtonDown(inputButton.Key))
                {
                    inputButton.Value.OnDown();
                }
                else if (UnityEngine.Input.GetButton(inputButton.Key))
                {
                    inputButton.Value.OnPress();
                }
                else if (UnityEngine.Input.GetButtonUp(inputButton.Key))
                {
                    inputButton.Value.OnUp();
                }
                else
                {
                    inputButton.Value.OnNotPress();
                }
            }
        }

        #region 입력 구현부

        private void GetAxisX(float value) => PlayerCharacter.boActor.moveDir.x = value;

        private void GetAxisZ(float value) => PlayerCharacter.boActor.moveDir.z = value;

        private void CameraRotation(float value)
        {
            mainCamera.RotateAround(PlayerCharacter.transform.position, Vector3.up, value * Define.Camera.RotSpeed);
        }

        public void MouseLeft() => PlayerCharacter.stateMachine.StateChange(State.Attack);

        private void ActiveInventoryUI() => UIPresenter.Instance.SetActiveInventoryUI();

        private void ActiveSaveDataUI() => UIPresenter.Instance.SetActiveSystemMenu();
        #endregion
    }
}