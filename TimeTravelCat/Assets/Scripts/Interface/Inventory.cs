using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    Slot[] slots = null;
    Slot draggedSlot = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        slots = transform.GetComponentsInChildren<Slot>();
        /*foreach(InventorySlot slot in slots) {
            Debug.Log("Slot = " + slot.name);
        }//*/
       //transform.GetComponentInChildren<HorizontalLayoutGroup>().enabled = false;
    }

    public bool AddItem(Interactable item) {
        Slot emptySlot;
        if ((emptySlot = GetFirstEmptySlot()) != null) {
            SlotItem slotItem = Instantiate(GameManager.Instance.prefabSlotItem, emptySlot.transform);
            slotItem.slot = emptySlot;
            slotItem.FromInteractable(item);
            emptySlot.AddItem(slotItem);
            item.Deactivate();
            return true;
        }
        Debug.Log("Failed to add to Inventory...");
        return false;
    }

    Slot GetFirstEmptySlot() {
        for(int i=0; i<slots.Length; i++) {
            if (slots[i].IsEmpty) {
                return slots[i];
            }
        }
        return null;
    }

    #region DragDrop

    public void BeginDragAction(Slot slot) {
        draggedSlot = slot;
    }

    public void EndDragAction() {
        draggedSlot = null;
    }

    public void DropAction(Slot slot) {
        //Debug.Log("draggedSlot: " + slot.name);
        if(draggedSlot == null) { return; }
        //Debug.Log("Dropped " + draggedSlot.name + " in " + slot.name);
        slot.ExchangeObjs(draggedSlot);
    }

    #endregion
}