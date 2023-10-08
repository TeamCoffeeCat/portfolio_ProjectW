using System.Diagnostics.CodeAnalysis;
using ProjectW.Resource;
using UnityEngine;

namespace ProjectW.Object
{
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// 현재 카메라 뷰 상태를 나타내는 필드
        /// </summary>
        public Define.Camera.View camView;

        /// <summary>
        /// 카메라 이동 시, 선형보간을 이용한 이동을 사용
        /// 선형보간 이동에 사용할 변위 값
        /// </summary>
        public float smooth = 3f;

        /// <summary>
        /// 캐릭터와의 거리를 참조할 객체
        /// </summary>
        private Transform cameraPos;

        /// <summary>
        /// 카메라가 추적할 타겟의 트랜스폼 참조(플레이어)
        /// </summary>
        private Transform target;

        /// <summary>
        /// 카메라 컴포넌트를 이용하여 서로 다른 좌표계에서 좌표변환을 이용해 연산을 해야 하는 경우가
        /// 프로젝트 내에서 빈번하게 발생하므로, 처음에 카메라 참조를 한 번 담아둔 다음 편리하게
        /// 접근하기 위하여 정적 필드를 생성
        /// </summary>
        public static Camera Cam { get; private set; }

        private void Start()
        {
            Cam = GetComponent<Camera>();
        }

        /// <summary>
        /// 카메라의 추적 타겟을 설정하는 기능
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            this.target = target;

            // 이 떄 추적하고자 하는 타겟에게 CamPos를 생성하여 하이라키 상의 자식으로 배치한다.
            //  -> CamPos에는 디폴트뷰와 프론트뷰를 갖는 자식이 존재하며, 이 때 생성한 CamPos를
            //     타겟을 기준(로컬포지션)으로 0,0,0에 배치한다면 미리 설정한 디폴트, 프론트 뷰에 따른
            //     위치와 회전 값을 갖게 됨
            cameraPos = Instantiate(ResourceManager.Instance.LoadObject(Define.Camera.CamPosPath)).transform;

            // CamPos의 부모를 타겟으로 설정
            cameraPos.SetParent(this.target);
            // 부모를 기준으로 0,0,0에 위치하도록
            cameraPos.localPosition = Vector3.zero;

            // 초기값 설정
            // 카메라의 위치와 방향을 처음에는 디폴트뷰로 설정
            Transform tr;
            (tr = transform).SetParent(cameraPos.GetChild(0));
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
        }
    }
}