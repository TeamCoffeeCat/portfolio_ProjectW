using CoffeeCat.Simplify;
using ProjectW.DB;
using System.Linq;
using UnityEngine;

namespace ProjectW.Object
{
    public class Item : MonoBehaviour
    {
        // 1000, 1001 hp
        // 1002, 1003 mp
        [SerializeField] private int index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // 인벤토리가 가득 찼으면 return
                if (GameManager.User.boInventory.IsFull())
                {
                    // 아이템창 가득 찼다 시스템 메세지
                    return;
                }

                AddItem();
                PoolManagerLight.Instance.Despawn(gameObject);
            }
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public void AddItem()
        {
            // boInventory에 아이템 추가
            GameManager.User.boInventory.AddItem(index);

            // UI Refresh
            UIPresenter.Instance.RefreshInventory();
        }
    }
}