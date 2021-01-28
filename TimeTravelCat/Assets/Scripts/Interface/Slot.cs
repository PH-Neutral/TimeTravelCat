using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
    public bool IsEmpty {
        get { return _slotItem == null; }
    }

    SlotItem _slotItem = null;

    private void Start() {
        FindSlotItem();
    }

    public void AddItem(SlotItem slotItem) {
        _slotItem = slotItem;
        ResetSlotItem();
    }

    #region DragDrop

    void DropOnScene(GameObject sceneElement) {
        Interactable element = sceneElement.GetComponent<Interactable>();
        if (element != null) {
            //Debug.Log("Dropped on: " + sceneElement.name);
            element.OnAction(Action.Receive, _slotItem);
        }
    }

    bool FindSlotItem() {
        if(transform.childCount > 0) {
            _slotItem = transform.GetComponentInChildren<SlotItem>();
            return true;
        }
        _slotItem = null;
        return false;
    }

    void ResetSlotItem() {
        if(!IsEmpty) {
            _slotItem.transform.SetParent(transform);
            _slotItem.transform.localPosition = Vector3.zero;
        }
    }

    public void ExchangeObjs(Slot other) {
        SlotItem temp = this._slotItem;
        this._slotItem = other._slotItem;
        other._slotItem = temp;
        this.ResetSlotItem();
        other.ResetSlotItem();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(!IsEmpty) {
            Inventory.Instance.BeginDragAction(this);
            //Debug.Log("Begin drag: " + name);
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (!IsEmpty) {
            _slotItem.transform.position = eventData.position;
            //Debug.Log("Drag: " + name);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Inventory.Instance.EndDragAction();
        if(!IsEmpty) {
            _slotItem.transform.localPosition = Vector3.zero;
            //Debug.Log("End drag: " + name);
            //Debug.Log("eventPos = " + eventData.position.ToString());
            Vector3 pointerPos = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(pointerPos, Vector2.zero, 15f, ~LayerMask.NameToLayer("Interactable"));
            if(hit.collider != null) {
                //Debug.Log("Raycast hit " + hit.collider.name);
                DropOnScene(hit.collider.gameObject);
            }
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Inventory.Instance.DropAction(this);
    }

    #endregion
}
