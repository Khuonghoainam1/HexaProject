using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class Hexagon : MonoBehaviour
    {
        [Header("Element")]
        [SerializeField] private new  Renderer m_renderer;
        public HexStack HexStack {  get; private set; }

        public Color Color
        {
            get => m_renderer.material.color;
            set => m_renderer.material.color = value;
        }

        public void Configure(HexStack hexStack)
        {
            HexStack = hexStack;
        }
    }
}
