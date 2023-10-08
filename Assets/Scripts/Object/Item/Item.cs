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
                // �κ��丮�� ���� á���� return
                if (GameManager.User.boInventory.IsFull())
                {
                    // ������â ���� á�� �ý��� �޼���
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
            // boInventory�� ������ �߰�
            GameManager.User.boInventory.AddItem(index);

            // UI Refresh
            UIPresenter.Instance.RefreshInventory();
        }
    }
}