using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class Tower : MonoBehaviour
    {
        [Header("Element")]
        [SerializeField] private Animator animator;
        [Header(" Setting")]
        [SerializeField] private float fillIncrement;
        private Renderer renderer;

        private float fillPercent;
        private void Awake()
        {

            animator.GetComponent<Animator>();
            renderer = GetComponent<Renderer>();
        }
        private void Start()
        {
          UpdateMaterials();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fill();
            }
        }

        private void Fill()
        {
            if (fillPercent >= 100)
            {
                return;

            }

            fillPercent += fillIncrement ;
            UpdateMaterials();
            animator.Play("Bump");


        }

        private void UpdateMaterials() 
        {
           foreach(Material material in renderer.materials)
            {
                material.SetFloat("_Fill_Percent", fillPercent);
            }
        }
    }
}
