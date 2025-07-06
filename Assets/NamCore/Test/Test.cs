using UnityEngine;

namespace NamCore
{
    /// <summary>
    /// Mô tả chức năng của class này. Ví dụ: Điều khiển hành vi nhân vật, xử lý hiệu ứng, v.v.
    /// </summary>
    public class Test : MonoBehaviour
    {
        #region Fields

        // Các biến thuộc tính private/protected
        // [SerializeField] private float speed = 5f;

        #endregion
        public Transform weaponTransform;
        private void Awake()
        {
            DataManager.Instance.LoadData();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
             /*   GameObject bullet = PoolManager.Instance.Spawn(
                    PoolerTarget.Combat,
                    PoolTyper.Bullet,
                    this.transform.position,
                    Quaternion.identity,
                    weaponTransform
                );*/
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                PopupManager.Instance.ShowPopup(PopupType.Settings, () => {
                    Debug.Log("Đã đóng popup xác nhận.");
                });
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                DataManager.Instance.Data.resources.Set(ResoucrType.coin, 10);
                Debug.Log(DataManager.Instance.Data.resources.Get(ResoucrType.coin));
            }
        }

 
    }
}

