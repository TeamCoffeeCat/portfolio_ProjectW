using Newtonsoft.Json;
using ProjectW.DB;
using ProjectW.Network;

namespace ProjectW.Dummy
{
    /// <summary>
    /// 실제 더미서버에서의 통신 프로토콜 구현부를 갖는 클래스
    /// </summary>
    public class ServerModuleDummy : INetworkClient
    {
        // db가 더미서버에 존재하고, 더미서버모듈은 더미서버에서 생성되므로
        // 처음 생성시 생성 주체(더미서버)의 참조를 받아서 이후 데이터 처리시에
        // 사용하게 한다.
        private DummyServer serverData;

        public ServerModuleDummy(DummyServer serverData)
        {
            this.serverData = serverData;
        }

        public void GetChatacter(int uniqueId, ResponseHandler<DtoCharacter> responseHandler)
        {
            responseHandler.HandleSuccess(JsonConvert.SerializeObject(serverData.userData.dtoCharacter));
        }

        public void GetStage(int uniqueId, ResponseHandler<DtoStage> responseHandler)
        {
            responseHandler.HandleSuccess(JsonConvert.SerializeObject(serverData.userData.dtoStage));
        }

        public void Login(int uniqueId, ResponseHandler<DtoAccount> responsHandler)
        {
            // 더미서버에서는 계정정보를 요청해서 어떻게 처리할 것인가에 대해 작성
            // 더미서버이므로 클라이언트에서 클라이언트로의 요청을 하는 것
            // 통신 요청에 대한 실패가 발생할 일이 없음
            //  -> 강제로 요청 성공 메서드를 실행시켜서 데이터(DB에 있는 데이터)를 넘겨줌
            responsHandler.HandleSuccess(JsonConvert.SerializeObject(serverData.userData.dtoAccount));
        }

        public void GetInventory(int uniqueId, ResponseHandler<DtoInventory> responseHandler)
        {
            responseHandler.HandleSuccess(JsonConvert.SerializeObject(serverData.userData.dtoInventory));
        }
    }
}
