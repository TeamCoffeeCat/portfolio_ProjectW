using ProjectW.DB;

namespace ProjectW.Network
{   
    /// <summary>
    /// 로그인시 필요한 데이터들을 서버에 요청하는 기능을 수행할 클래스
    /// </summary>
    public class LoginHandler
    {
        /// <summary>
        /// 요청 후 응답처리에 사용할 핸들러 객체
        /// </summary>
        public ResponseHandler<DtoAccount> accountResponse;
        public ResponseHandler<DtoCharacter> characterResponse;
        public ResponseHandler<DtoStage> stageResponse;
        public ResponseHandler<DtoInventory> inventoryResponse;

        public LoginHandler()
        {
            accountResponse = new ResponseHandler<DtoAccount>(GetAccountSuccess, OnFailed);
            characterResponse = new ResponseHandler<DtoCharacter>(GetCharacterSuccess, OnFailed);
            stageResponse = new ResponseHandler<DtoStage>(GetStageSuccess, OnFailed);
            inventoryResponse = new ResponseHandler<DtoInventory>(GetInventorySuccess, OnFailed);
        }

        /// <summary>
        /// 로그인 요청(유저 데이터 요청) 기능
        /// </summary>
        public void Connect()
        {
            ServerManager.Server.Login(0, accountResponse);
            ServerManager.Server.GetChatacter(0, characterResponse);
            ServerManager.Server.GetStage(0, stageResponse);
            ServerManager.Server.GetInventory(0, inventoryResponse);
        }

        /// <summary>
        /// 캐릭터 정보 요청 성공시 실행할 메서드
        /// </summary>
        /// <param name="dtoCharacter"></param>
        public void GetCharacterSuccess(DtoCharacter dtoCharacter)
        {
            GameManager.User.boCharacter = new BoCharacter(dtoCharacter);
        }

        /// <summary>
        /// 계정 정보 요청 성공시 실행할 메서드
        /// </summary>
        /// <param name="dtoAccount"></param>
        public void GetAccountSuccess(DtoAccount dtoAccount)
        {
            GameManager.User.boAccount = new BoAccount(dtoAccount);
        }

        /// <summary>
        /// 스테이지 정보 요청 성공 시 실행할 메서드
        /// </summary>
        /// <param name="dtoStage"></param>
        public void GetStageSuccess(DtoStage dtoStage)
        {
            GameManager.User.boStage = new BoStage(dtoStage);
        }

        /// <summary>
        /// 인벤토리 정보 요청 성공 시 실행할 메서드
        /// </summary>
        /// <param name="dtoInventory"></param>
        public void GetInventorySuccess(DtoInventory dtoInventory)
        {
            GameManager.User.boInventory = new BoInventory(dtoInventory);
        }

        /// <summary>
        /// 요청 실패시 실행할 메서드
        /// </summary>
        /// <param name="dtoBase"></param>
        public void OnFailed(DtoBase dtoBase)
        {

        }

    }
}
