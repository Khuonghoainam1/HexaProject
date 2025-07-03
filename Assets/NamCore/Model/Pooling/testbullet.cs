using UnityEngine;

namespace NameCore
{
    /// <summary>
    /// Mô tả chức năng của class này. Ví dụ: Điều khiển hành vi nhân vật, xử lý hiệu ứng, v.v.
    /// </summary>
    public class testbullet : MonoBehaviour, IPoolable
    {
        private float timer = 0f;
        private bool isActive = false;

        public void OnSpawn()
        {
            // Reset timer và trạng thái khi bullet được spawn
            timer = 0f;
            isActive = true;
        }

        public void OnDespawn()
        {
            // Đặt lại trạng thái khi bullet được despawn
            isActive = false;
        }

        private void Update()
        {
            if (isActive)
            {
                // Tăng timer mỗi frame
                timer += Time.deltaTime;

                // Tự động despawn sau 3 giây
                if (timer >= 3f)
                {
                    PoolManager.Instance.Despawn(gameObject);
                }
            }
        }
    }
}

