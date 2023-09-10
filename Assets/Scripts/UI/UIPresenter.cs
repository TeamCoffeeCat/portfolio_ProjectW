using ProjectW.Object;
using ProjectW.Resource;
using ProjectW.Util;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectW
{
    public class UIPresenter : Singleton<UIPresenter>
    {
        [SerializeField] private SaveDataUI saveDataUI;
        public RectTransform CursorIcon;
        private IngameUI ingameUI;
        private InventoryUI inventoryUI;

        // TO DO : 시스템 메세지
        // ex) 아이템 칸 부족, 아이템 획득, 레벨업 등

        private void Start()
        {
            saveDataUI.Initialize();
            InitCursor();
        }

        private void Update()
        {
            UpdateCursorIcon();
        }

        public void InitUI(Character character)
        {
            InitIngameUI(character);
            InitInventoryUI();
            saveDataUI.SetSaveMode();
        }

        private void InitIngameUI(Character character)
        {
            var ingame = Instantiate(ResourceManager.Instance.LoadObject("Prefabs/UI/IngameUI"));
            ingameUI = ingame.GetComponent<IngameUI>();
            ingameUI.Init(character);
            ingameUI.SaveButton.onClick.AddListener(() =>
            {
                ingameUI.SystemMenu.SetActive(false);
                saveDataUI.gameObject.SetActive(true); 
            });
            ingameUI.QuitButton.onClick.AddListener(() => Application.Quit());
        }

        private void InitInventoryUI()
        {
            var inventory = Instantiate(ResourceManager.Instance.LoadObject("Prefabs/UI/InventoryUI"));
            inventory.SetActive(false);
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
            Vector2 mousePos = Input.mousePosition;
            CursorIcon.position = mousePos + new Vector2(13f, -25f);
        }
    }
}