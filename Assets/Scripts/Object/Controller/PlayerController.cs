using ProjectW.Controller;
using UnityEngine;
using InputAxisEvent = ProjectW.Controller.InputController.InputAxisEvent;
using State = ProjectW.Define.Actor.State;

namespace ProjectW.Object
{
    using Input = Define.Input;

    /// <summary>
    /// �÷��̾� ĳ������ �Է�ó��
    /// ĳ���� Ŭ�������� ó�� ���ϴ� ����?
    /// ĳ���Ϳ� �÷��̾� �Է��� �и������ν�
    /// ĳ���� Ŭ������ �� �پ��ϰ� ����� �� �ֱ� ������ (ex: ��Ƽ ȯ�濡���� ĳ���� ���� ��)
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private InputController inputController;

        public Character PlayerCharacter { get; private set; }

        private Transform mainCamera;

        /// <summary>
        /// �ʱ�ȭ ��, �÷��̾� ĳ������ ������ ���Թ޴´�.
        /// </summary>
        /// <param name="character"></param>
        public void Initialize(Character character)
        {
            transform.position = character.transform.position;
            mainCamera = Camera.main.transform;

            // �÷��̾� ĳ������ ���̶�Ű ���� �θ� �÷��̾� ��Ʈ�ѷ��� ����
            character.transform.SetParent(transform);
            // �÷��̾� ĳ������ �±׸� Player �±׷� ����
            // (�ٸ� ĳ���Ͱ� �����Ѵٰ� �������� ��, ���ϰ� �÷��̾��� ĳ���͸� �����ϱ� ����)
            character.gameObject.layer = LayerMask.NameToLayer("Player");

            // �÷��̾� ĳ���� ���� ����
            PlayerCharacter = character;

            // �Է� ��Ʈ�ѷ� ��ü ����
            inputController = new InputController();

            // �� Ÿ�� Ű ���
            inputController.AddAxis(Input.AxisX, new InputAxisEvent(GetAxisX)); // ĳ���� ��, �� �̵�
            inputController.AddAxis(Input.AxisZ, new InputAxisEvent(GetAxisZ)); // ĳ���� ��, �� �̵�
            inputController.AddAxis(Input.MouseX, new InputAxisEvent(CameraRotation)); // ī�޶� ȸ��

            // ��ư Ÿ�� Ű ���
            // inputController.AddButton(Input.Jump, // ĳ���� ����
            //     downEvent: new InputButtonEvent(SpaceDown));
            
            // inputController.AddButton(Input.MouseLeft, // ����
            //     downEvent: new InputButtonEvent(MouseLeft),
            //     pressEvent: new InputButtonEvent(MouseLeft));
        }

        private void FixedUpdate()
        {
            // �÷��̾� ĳ���Ͱ� ���õǱ� ������ �Է��� �� ������
            if (PlayerCharacter == null)
                return;

            // �÷��̾ �׾��ٸ� �Է��� �� ������
            if (PlayerCharacter.stateMachine.state == State.Die)
                return;

            InputUpdate();
            ActiveInventoryUI();
            ActiveSaveDataUI();
        }

        private void InputUpdate()
        {
            // �� Ÿ���� Ű üũ
            foreach (var inputAxis in inputController.inputAxes)
            {
                inputAxis.Value.GetAxisValue(UnityEngine.Input.GetAxisRaw(inputAxis.Key));
            }

            // ��ư Ÿ���� Ű üũ
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

        #region �Է� ������
        public void GetAxisX(float value) => PlayerCharacter.boActor.moveDir.x = value;

        public void GetAxisZ(float value) => PlayerCharacter.boActor.moveDir.z = value;

        public void CameraRotation(float value)
        {
            mainCamera.RotateAround(PlayerCharacter.transform.position, Vector3.up, value * Define.Camera.RotSpeed);
        }

        public void MouseLeft() => PlayerCharacter.stateMachine.StateChange(State.Attack);

        public void ActiveInventoryUI() => UIPresenter.Instance.SetActiveInventoryUI();

        public void ActiveSaveDataUI() => UIPresenter.Instance.SetActiveSystemMenu();
        #endregion
    }
}