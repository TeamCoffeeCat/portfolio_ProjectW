using System.Diagnostics.CodeAnalysis;
using ProjectW.Object;
using ProjectW.Resource;
using ProjectW.Util;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class UIPresenter : Singleton<UIPresenter>
    {
        [SerializeField] private SaveDataUI saveDataUI;
        public RectTransform CursorIcon;
        private IngameUI ingameUI;
        private InventoryUI inventoryUI;

        private readonly string ingameUIpath = "Prefabs/UI/IngameUI";
        private readonly string inventoryUIpath = "Prefabs/UI/InventoryUI";

        private void Start()
        {
            saveDataUI.Initialize();
            InitCursor();
        }

        private void Update()
        {
            UpdateCursorIcon();
        }

        // Ingame에서 사용 할 UI의 Initialize
        public void InitUI(Character character)
        {
            InitIngameUI(character);
            InitInventoryUI();
            saveDataUI.SetSaveMode();
        }

        private void InitIngameUI(Character character)
        {
            // 인게임에서 사용 할 UI 복사생성
            var ingame = Instantiate(ResourceManager.Instance.LoadObject(ingameUIpath));
            // UI를 컨트롤 할 클래스에 접근하여 초기화
            ingameUI = ingame.GetComponent<IngameUI>();
            ingameUI.Init(character);
            
            // 버튼에 이벤트 할당
            ingameUI.SaveButton.onClick.AddListener(() =>
            {
                ingameUI.SystemMenu.SetActive(false);
                saveDataUI.gameObject.SetActive(true); 
            });
            ingameUI.QuitButton.onClick.AddListener(Application.Quit);
        }

        private void InitInventoryUI()
        {
            // 인벤토리 UI 복사생성
            var inventory = Instantiate(ResourceManager.Instance.LoadObject(inventoryUIpath));
            // 상시 표출 할 UI가 아니므로 비활성화
            inventory.SetActive(false);
            // UI를 컨트롤 할 클래스에 접근하여 초기화
            inventoryUI = inventory.GetComponent<InventoryUI>();
            inventoryUI.Init();
        }

        public void RefreshInventory()
        {
            for (int i = 0; i < inventoryUI.boInventory.BoItems.Count; i++)
            {
                inventoryUI.itemSlots[i].RefreshSlot(inventoryUI.boInventory.BoItems[i]);
            }
        }

        public void SetActiveSaveDataUI(bool isActive)
        {
            saveDataUI.gameObject.SetActive(isActive);
        }

        public void ActiveRespawnMessage()
        {
            ingameUI.RespawnMessage.SetActive(true);
        }

        public void SetActiveInventoryUI()
        {
            if (Input.GetKeyDown(KeyCode.I))
                inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }

        public void SetActiveSystemMenu()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ingameUI.SystemMenu.SetActive(!ingameUI.SystemMenu.activeSelf);
        }

        private void InitCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            if (CursorIcon.GetComponent<Graphic>())
                CursorIcon.GetComponent<Graphic>().raycastTarget = false;
        }

        private void UpdateCursorIcon()
        {
            Cursor.visible = false;
            Vector2 mousePos = Input.mousePosition;
            CursorIcon.position = mousePos + new Vector2(13f, -25f);
        }
    }
}