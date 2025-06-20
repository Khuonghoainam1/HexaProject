using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class HexStack : MonoBehaviour
    {
        public List<Hexagon> Hexagons { get; private set; }

        public Color GetHexagonColor() => Hexagons[^1].Color;
     

        public void Add(Hexagon hexagon)
        {
            if (Hexagons == null)
            { Hexagons = new List<Hexagon>(); }


            Hexagons.Add(hexagon);
        }

        public void Place()
        {
            foreach (Hexagon hexagon in Hexagons)
            {
                hexagon.DisableCollider();
            }
        }

    }
}
