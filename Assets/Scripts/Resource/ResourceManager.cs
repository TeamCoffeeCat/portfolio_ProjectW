using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Resource
{
    /// <summary>
    /// 런타임(실행시간)에 필요한 리소스를 불러오는 기능을 담당할 클래스
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        public void Initialize()
        {

        }

        /// <summary>
        /// Assets/Resources 폴더 내의 프리팹을 불러와 반환하는 기능
        /// </summary>
        /// <param name="path">Resources 폴더 내 불러올 에셋 경로</param>
        /// <returns>불러온 게임오브젝트</returns>
        public GameObject LoadObject(string path)
        {
            // Resources.Load -> Assets 폴더 내 Resources 라는 이름의 폴더가 존재한다면
            // 해당 경로부터 path를 읽음, 해당 경로에 파일이 GameObject 형태로 부를 수 있다면 불러옴
            return Resources.Load<GameObject>(path);
        }

        /// <summary>
        /// 오브젝트 풀로 사용할 객체의 프리팹을 로드 후, 오브젝트 풀 매니저를 이용하여 풀을 등록하는 기능
        /// </summary>
        /// <typeparam name="T">로드하고자 하는 타입</typeparam>
        /// <param name="path">프리팹 경로</param>
        /// <param name="poolCount">풀 등록시, 초기 인스턴스 수</param>
        /// <param name="loadComplete">로드 및 등록 완료 후 실행시킬 이벤트</param>
        public void LoadPoolableObject<T>(string path, int poolCount = 1, Action loadComplete = null) where T : MonoBehaviour, IPoolableObject
        {
            // 프리팹을 로드한다.
            var obj = LoadObject(path);
            // 프리팹이 갖고 있는 T타입 컴포넌트 참조를 가져온다.
            var tComponent = obj.GetComponent<T>();

            // t타입의 풀을 등록
            ObjectPoolManager.Instance.RegistPool<T>(tComponent, poolCount);

            // 위 작업이 모두 끝난 후, 실행시킬 기능이 있다면 실행
            loadComplete?.Invoke();

        }
    }
}