using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Controller
{
    /// <summary>
    /// �Է� ó���� ����� Ŭ����
    /// </summary>
    public class InputController
    {
        // ũ�� 2���� Ÿ������ �Է� ó��
        //  -> �� Ÿ���� Ű
        //  -> ��ư Ÿ���� Ű

        /// <summary>
        /// ��ư Ÿ���� Ű �Է� ó�� �� ������ �޼��带 �븮�� �븮��
        /// </summary>
        public delegate void InputButtonEvent();

        /// <summary>
        /// �� Ÿ���� Ű �Է� ó�� �� ������ �޼��带 �븮�� �븮��
        /// </summary>
        public delegate void InputAxisEvent(float value);

        /// <summary>
        /// string ���� ����� Ű�� �̸�
        /// AxisHandler/buttonHandler ����� Ű�� ������ ��/���� �� �� �پ��� ��Ȳ��
        /// �߻���ų ����� ���� �ִ� Handler
        /// </summary>
        public List<KeyValuePair<string, AxisHandler>> inputAxes = new List<KeyValuePair<string, AxisHandler>>();
        public List<KeyValuePair<string, ButtonHandler>> inputButtons = new List<KeyValuePair<string, ButtonHandler>>();

        /// <summary>
        /// �� Ÿ���� Ű�� �ش� Ű ��ȣ�ۿ� �� �����ų ��� ���� ����ϴ� ���
        /// </summary>
        /// <param name="key">��Ͻ�Ű���� �ϴ� ��Ÿ���� Ű</param>
        /// <param name="axisEvent">����� Ű�� ��ȣ�ۿ� �� ������ ���</param>
        public void AddAxis(string key, InputAxisEvent axisEvent)
        {
            inputAxes.Add(new KeyValuePair<string, AxisHandler>(key, new AxisHandler(axisEvent)));
        }

        public void AddButton(string key, InputButtonEvent downEvent = null, InputButtonEvent upEvent = null,
                InputButtonEvent pressEvent = null, InputButtonEvent notPressEvent = null)
        {
            inputButtons.Add(new KeyValuePair<string, ButtonHandler>(key,
                new ButtonHandler(downEvent, upEvent, pressEvent, notPressEvent)));
        }


        public class AxisHandler
        {
            private InputAxisEvent axisEvent;

            public AxisHandler(InputAxisEvent axisEvent)
            {
                this.axisEvent = axisEvent;
            }

            public void GetAxisValue(float value)
            {
                axisEvent?.Invoke(value);
            }
        }

        public class ButtonHandler
        {
            private InputButtonEvent downEvent; // ��ϵ� ��ư�� ���� ���� (1��) �����ų ���
            private InputButtonEvent upEvent; // ��ϵ� ��ư�� �� ���� (1��) �����ų ���
            private InputButtonEvent pressEvent; // ��ϵ� ��ư�� ������ �ִ� ���� (������) �����ų ���
            private InputButtonEvent notPressEvent; // ��ϵ� ��ư�� ���� �ִ� ���� (������) �����ų ���

            /// <summary>
            /// ��ư �ڵ鷯 ���� �� �ڵ鷯�� �븮�� ��ɵ��� �޾ƿ´�.
            /// </summary>
            /// <param name="downEvent"></param>
            /// <param name="upEvent"></param>
            /// <param name="pressEvent"></param>
            /// <param name="notPressEvent"></param>
            public ButtonHandler(InputButtonEvent downEvent, InputButtonEvent upEvent,
                InputButtonEvent pressEvent, InputButtonEvent notPressEvent)
            {
                this.downEvent = downEvent;
                this.upEvent = upEvent;
                this.pressEvent = pressEvent;
                this.notPressEvent = notPressEvent;
            }

            public void OnDown()
            {
                downEvent?.Invoke();
            }

            public void OnUp()
            {
                upEvent?.Invoke();
            }

            public void OnPress()
            {
                pressEvent?.Invoke();
            }

            public void OnNotPress()
            {
                notPressEvent?.Invoke();
            }
        }
    }
}