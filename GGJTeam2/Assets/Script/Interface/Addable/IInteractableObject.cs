using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObject
{
    /* Overview
     * bool IsInteractable
     * - Decide if an object can be interacted or not
     * 
     * string InteractionText
     * - Text that appear when player hover over object
     * 
     * void SetupInteractable()
     * - Set up IInteractableObject
     * 
     * IEnumerator ExecuteInteraction()
     * - When the player hover over an Gameobject that implements
     * - IInteractableObject and IsInteractable is true, then it
     * - execute the Interaction specific to that object
     */

    bool IsInteractable { get; set; }
    string InteractionText { get; set; }

    void SetupInteractable();
    IEnumerator ExecuteInteraction();

}
