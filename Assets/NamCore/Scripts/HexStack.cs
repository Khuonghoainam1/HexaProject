using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamCore
{
    public class HexStack : MonoBehaviour
    {
        // Public property to access the list of hexagons.
        // The setter is private, meaning only HexStack can modify the list itself.
        public List<Hexagon> Hexagons { get; private set; }

        // Gets the ColorID of the topmost hexagon in the stack.
        // Uses the C# 8.0 range operator [^1] for the last element.
        public ColorID GetColorID()
        {
            if (Hexagons == null || Hexagons.Count == 0)
            {
                Debug.LogWarning("HexStack is empty, cannot get ColorID.");
                return ColorID.None; // Return a default or error value
            }
            return Hexagons[^1].colorID;
        }

        /// <summary>
        /// Initializes the HexStack by collecting existing Hexagon children.
        /// This is useful if hexagons are pre-placed in the scene or loaded as children.
        /// </summary>
        public void Initialize()
        {
            if (Hexagons == null)
            {
                Hexagons = new List<Hexagon>();
            }
            else
            {
                Hexagons.Clear(); // Clear any existing list before re-initializing
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Hexagon hexagon = transform.GetChild(i).GetComponent<Hexagon>();
                if (hexagon != null)
                {
                    Add(hexagon); // Add uses SetParent, ensuring consistency
                }
                else
                {
                    Debug.LogWarning($"Child {i} of {gameObject.name} is not a Hexagon and will be ignored in HexStack initialization.");
                }
            }
            Place(); // Apply 'Place' logic after initialization
        }

        /// <summary>
        /// Adds a Hexagon to the top of the stack.
        /// </summary>
        /// <param name="hexagon">The Hexagon to add.</param>
        public void Add(Hexagon hexagon)
        {
            if (Hexagons == null)
            {
                Hexagons = new List<Hexagon>();
            }
            Hexagons.Add(hexagon);

            // Set the parent of the hexagon to this HexStack's transform.
            // This is essential for managing hierarchy and relative positions.
            hexagon.SetParent(transform);
            // You might want to set localPosition here based on stack height:
            // hexagon.transform.localPosition = Vector3.up * (Hexagons.Count - 1) * .2f;
        }

        /// <summary>
        /// Applies initial 'placement' logic to all hexagons in the stack,
        /// such as disabling colliders.
        /// </summary>
        public void Place()
        {
            if (Hexagons == null) return;

            foreach (Hexagon hexagon in Hexagons)
            {
                if (hexagon != null)
                {
                    // Assuming Hexagon has a DisableCollider() method
                    hexagon.DisableCollider();
                }
            }
        }

        /// <summary>
        /// Checks if a specific Hexagon is present in this stack.
        /// </summary>
        /// <param name="hexagon">The Hexagon to check for.</param>
        /// <returns>True if the Hexagon is in the stack, false otherwise.</returns>
        public bool Contains(Hexagon hexagon)
        {
            return Hexagons != null && Hexagons.Contains(hexagon);
        }

        /// <summary>
        /// Removes a specific Hexagon from the stack.
        /// If the stack becomes empty after removal, the HexStack GameObject is destroyed.
        /// </summary>
        /// <param name="hexagon">The Hexagon to remove.</param>
        public void Remove(Hexagon hexagon)
        {
            if (Hexagons == null) return;

            if (Hexagons.Remove(hexagon)) // Returns true if item was found and removed
            {
                // Optionally, you might want to destroy the hexagon's GameObject here
                // if it's no longer needed in the scene.
                // if (hexagon != null)
                // {
                //     if (Application.isPlaying) Destroy(hexagon.gameObject);
                //     else DestroyImmediate(hexagon.gameObject);
                // }

                if (Hexagons.Count <= 0)
                {
                    // Destroy the HexStack GameObject if it's empty.
                    // Use DestroyImmediate in editor, Destroy in play mode.
                    if (Application.isPlaying)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        DestroyImmediate(gameObject);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Attempted to remove Hexagon '{hexagon?.name}' but it was not found in HexStack '{gameObject.name}'.");
            }
        }

        /// <summary>
        /// Clears all hexagons from the stack, destroying their GameObjects.
        /// Handles destruction safely for both Editor and Runtime.
        /// This method is primarily used by GridCell/GridManager for loading/clearing grids.
        /// </summary>
        public void ClearHexagons()
        {
            if (Hexagons == null) return;

            // Iterate backwards to safely remove elements while iterating
            for (int i = Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hex = Hexagons[i];
                if (hex != null) // Ensure the object still exists
                {
                    if (Application.isPlaying)
                    {
                        Destroy(hex.gameObject); // Destroy GameObject in runtime
                    }
                    else
                    {
                        DestroyImmediate(hex.gameObject); // Destroy GameObject immediately in Editor
                    }
                }
            }
            Hexagons.Clear(); // Clear all references from the list
            // Note: This method only clears the *hexagons*, not the HexStack GameObject itself.
            // The HexStack GameObject is destroyed by GridCell.ClearStack() or Remove() if it becomes empty.
        }

        /// <summary>
        /// Provides a list of hexagons in the stack. Used by GridCell for saving data.
        /// This is the getter for the 'Hexagons' property, but named as a method for clarity
        /// when used by external classes.
        /// </summary>
        /// <returns>A list of Hexagon objects currently in the stack.</returns>
        public List<Hexagon> GetHexagons()
        {
            return Hexagons;
        }

        // It's good practice to ensure Hexagons list is initialized on Awake/Start
        // if it's not guaranteed to be initialized by other methods like Initialize().
        private void Awake()
        {
            if (Hexagons == null)
            {
                Hexagons = new List<Hexagon>();
            }
            // If you want auto-initialization from children, you could call Initialize() here
            // if you expect HexStack to always start with pre-existing children.
            // Otherwise, keep Initialize() as a separate call for specific scenarios.
        }
    }
}