using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class Hexagon : MonoBehaviour
    {
        [Header("Element")]
        [SerializeField] private new  Renderer m_renderer;
        [SerializeField] private new  Collider m_collider;
        bool isActive ; 

        public ColorID colorID;
        public HexStack HexStack {  get; private set; }

     /*   public Color Color
        {
            get => m_renderer.material.color;
            set => m_renderer.material.color = value;
        }
*/
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Configure(HexStack hexStack)
        {
            HexStack = hexStack;
        }

        public void DisableCollider() => m_collider.enabled = false;

        public void Vanish(float delay)
        {
            LeanTween.cancel(gameObject);

            LeanTween.scale(gameObject, Vector3.zero, .2f)
                .setEase(LeanTweenType.easeInBack)
                .setDelay(delay)
                .setOnComplete(() => Destroy(gameObject));


        }
        public void MoveToLocal(Vector3 targetLocalPos)
        {
            LeanTween.cancel(gameObject); 
            float delay = transform.GetSiblingIndex() * .01f;

            LeanTween.moveLocal(gameObject, targetLocalPos, .2f)
                .setEase(LeanTweenType.easeInOutSine)
                .setDelay(transform.GetSiblingIndex() * .01f);

            Vector3 direction  = (targetLocalPos - transform.localPosition).With(y: 0).normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
            LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
                .setEase(LeanTweenType.easeOutSine)
                .setDelay(delay);
        }

        public void GetColor()
        {
            m_renderer.material.color = GameManager.Ins.levelData.configLevelData[0].GetColorByID(colorID);
        }

        
    }
}
