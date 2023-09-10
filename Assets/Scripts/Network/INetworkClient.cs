using ProjectW.DB;

namespace ProjectW.Network
{
    /// <summary>
    /// 모든 서버 모듈은 해당 인터페이스를 상속받는다.
    /// 해당 인터페이스는 서버와 통신하는 프로토콜 메서드를 갖는다.
    /// 프로토콜이란?
    /// 서버와 클라이언트 간의 통신에 사용되는 API
    /// </summary>
    public interface INetworkClient
    {
        /// <summary>
        /// 서버에 계정 정보를 요청하는 메서드
        /// </summary>
        /// <param name="uniqueId">
        /// 서버에 계정 정보 요청시 어떤 유저에 대한 계정 정보를 원하는지?
        /// 각 계정마다 부여된 고유한 키 값을 통해 유저를 식별</param>
        /// <param name="responsHandler">
        /// 서버에 요청한 데이터를 받았을 때, 어떻게 처리할 것인지에 대한 정보를 갖는 핸들러</param>
        void Login(int uniqueId, ResponseHandler<DtoAccount> responsHandler);

        /// <summary>
        /// 서버에 캐릭터 정보를 요청하는 메서드
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="responseHandler"></param>
        void GetChatacter(int uniqueId, ResponseHandler<DtoCharacter> responseHandler);

        /// <summary>
        /// 서버에 스테이지 정보를 요청하는 메서드
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="responseHandler"></param>
        void GetStage(int uniqueId, ResponseHandler<DtoStage> responseHandler);

        /// <summary>
        /// 서버에 인벤토리 정보를 요청하는 메서드 ?
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="responseHandler"></param>
        void GetInventory(int uniqueId, ResponseHandler<DtoInventory> responseHandler);
    }
}
