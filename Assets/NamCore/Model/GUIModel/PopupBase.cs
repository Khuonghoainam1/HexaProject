using System;
using UnityEngine;
namespace NamCore
{
    public abstract class PopupBase : MonoBehaviour
    {
        public Action onCloseCallback;

        [Header("Animation")]
        public Animator animator;

        protected virtual void OnEnable()
        {
            if (animator) animator.SetTrigger("Open");
        }

        public virtual void Close()
        {
            if (animator)
            {
                animator.SetTrigger("Close");
                StartCoroutine(WaitAndDeactivate());
            }
            else
            {
                Deactivate();
            }
        }

        private System.Collections.IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(0.3f); // Thời gian trùng với animation
            Deactivate();
        }

        private void Deactivate()
        {
            onCloseCallback?.Invoke();
            gameObject.SetActive(false);
        }
    }

}

