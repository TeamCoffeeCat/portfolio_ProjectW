using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Controller
{
    /// <summary>
    /// 입력 처리를 담당할 클래스
    /// </summary>
    public class InputController
    {
        // 크게 2가지 타입으로 입력 처리
        //  -> 축 타입의 키
        //  -> 버튼 타입의 키

        /// <summary>
        /// 버튼 타입의 키 입력 처리 시 실행할 메서드를 대리할 대리자
        /// </summary>
        public delegate void InputButtonEvent();

        /// <summary>
        /// 축 타입의 키 입력 처리 시 실행할 메서드를 대리할 대리자
        /// </summary>
        public delegate void InputAxisEvent(float value);

        /// <summary>
        /// string 내가 등록한 키의 이름
        /// AxisHandler/buttonHandler 등록한 키를 눌렀을 때/뗐을 때 등 다양한 상황에
        /// 발생시킬 기능을 갖고 있는 Handler
        /// </summary>
        public List<KeyValuePair<string, AxisHandler>> inputAxes = new List<KeyValuePair<string, AxisHandler>>();
        public List<KeyValuePair<string, ButtonHandler>> inputButtons = new List<KeyValuePair<string, ButtonHandler>>();

        /// <summary>
        /// 축 타입의 키와 해당 키 상호작용 시 실행시킬 기능 등을 등록하는 기능
        /// </summary>
        /// <param name="key">등록시키고자 하는 축타입의 키</param>
        /// <param name="axisEvent">등록한 키와 상호작용 시 실행할 기능</param>
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
            private InputButtonEvent downEvent; // 등록된 버튼을 누른 순간 (1번) 실행시킬 기능
            private InputButtonEvent upEvent; // 등록된 버튼을 뗀 순간 (1번) 실행시킬 기능
            private InputButtonEvent pressEvent; // 등록된 버튼을 누르고 있는 상태 (여러번) 실행시킬 기능
            private InputButtonEvent notPressEvent; // 등록된 버튼을 떼고 있는 상태 (여러번) 실행시킬 기능

            /// <summary>
            /// 버튼 핸들러 생성 시 핸들러가 대리할 기능들을 받아온다.
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