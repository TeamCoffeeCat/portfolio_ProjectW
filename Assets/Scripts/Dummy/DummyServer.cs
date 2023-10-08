using ProjectW.Network;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectW.Dummy
{
    // 더미서버의 역할을 수행할 클래스
    // 더미서버에서 사용할 db를 갖는다.
    public class DummyServer : Singleton<DummyServer>
    {
        // 더미서버에서 갖는 유저데이터 (유저 DB)
        public UserData userData;

        // 더미서버의 통신 기능을 갖는 모듈
        public INetworkClient dummyModule;

        public void Initialize()
        {
            dummyModule = new ServerModuleDummy(this);
        }
    }
}