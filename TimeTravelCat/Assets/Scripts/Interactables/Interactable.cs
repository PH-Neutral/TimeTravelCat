using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    public Action[] AvailableActions {
        get {
            List<Action> actions = new List<Action>();
            if (canBeInspected) { actions.Add(Action.Inspect); }
            if (canBePickedUp) { actions.Add(Action.PickUp); }
            if (canBeUsed) { actions.Add(Action.Use); }
            return actions.ToArray();
        }
    }
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public Color color;

    [SerializeField] InteractableType type = InteractableType.None;
    [SerializeField] Interactable receivableItem = null;
    [SerializeField] bool canReceive = false;
    [SerializeField] bool softBlockReceive = false;
    [SerializeField] bool canBeInspected = false;
    [SerializeField] bool softBlockInspect = false;
    [SerializeField] bool canBePickedUp = false;
    [SerializeField] bool softBlockPickUp = false;
    [SerializeField] bool canBeUsed = false;
    [SerializeField] bool softBlockUse = false;
    [SerializeField] UnityEvent eventReceive = null;
    [SerializeField] UnityEvent eventInspect = null;
    [SerializeField] UnityEvent eventPickUp = null;
    [SerializeField] UnityEvent eventUse = null;
    ActionWheel actionWheel = null;
    SpriteRenderer sRend;
    Collider2D col; 

    protected virtual void Awake() {
        sRend = GetComponentInChildren<SpriteRenderer>();
        col = GetComponentInChildren<Collider2D>();
        sprite = sRend.sprite;
        color = sRend.color;
    }

    private void Update() {
        if(GameManager.Instance.GamePaused) { return; }
        if (Input.GetMouseButtonDown(0)) {
            if (col.HasBeenClickedOn(LayerMask.NameToLayer("Interactable"))) {
                Interact();
            }
        }
    }

    void Interact() {
        //if (!IsActive) { return; }
        Action[] actions = AvailableActions;
        if(actions.Length == 1) {
            OnAction(actions[0]);
        } else if (actions.Length > 1) {
            if(actionWheel == null) {
                actionWheel = SpawnActionWheel();
            }
        }
    }

    ActionWheel SpawnActionWheel() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        ActionWheel wheel = Instantiate(GameManager.Instance.prefabActionWheel, mousePos, Quaternion.identity);
        wheel.Item = this;
        return wheel;
    }

    public void MakeVisible(bool visible) {
        sRend.enabled = visible;
    }

    public void MakeInteractable(bool activateCollider = true) {
        col.enabled = activateCollider;
    }

    public void OnAction(Action action, SlotItem item = null) {
        //if(!IsActive) { return false; }
        bool success;
        switch (action) {
            case Action.Receive:
                if (!canReceive) { return; }
                success = Receive(item);
                break;
            case Action.Inspect:
                if(!canBeInspected) { return; }
                success = Inspect();
                break;
            case Action.PickUp:
                if(!canBePickedUp) { return; }
                success = PickUp();
                break;
            case Action.Use:
                if(!canBeUsed) { return; }
                success = Use();
                break;
            default:
                return;
        }
        InteractableType otherType = item != null ? item.Item.type : InteractableType.None;
        GameManager.Instance.StartDialogue(action, type, otherType, success);
    }

    bool Receive(SlotItem slotItem) {
        if(softBlockReceive) { return false; }
        //Debug.Log(name + " : OnReceive(item = " + slotItem.Item.name + ")"); 
        if (receivableItem == slotItem.Item) {
            slotItem.ToInteractable(transform.position);
            Destroy(slotItem.gameObject);
            eventReceive.Invoke();
            MakeInteractable(false);
            return true;
        }
        return false;
    }

    bool Inspect() {
        if(softBlockInspect) { return false; }
        //Debug.Log(name + " : OnInspect()");
        eventInspect.Invoke();
        return true;
    }

    bool PickUp() {
        if(softBlockPickUp) { return false; }
        bool EnoughPlace = Inventory.Instance.AddItem(this);
        //Debug.Log(name + " : OnPickUp(enoughPlace = " + EnoughPlace + ")");
        eventPickUp.Invoke();
        return true;
    }

    bool Use() {
        if(softBlockUse) { return false; }
        //Debug.Log(name + " : OnUse()");
        eventUse.Invoke();
        return true;
    }
    /*/
    protected virtual void OnReceive(SlotItem item) {
        if (eventReceive != null) {
            eventReceive.Invoke();
        }
    }

    protected virtual void OnInspect() {
        if(eventInspect != null) {
            eventInspect.Invoke();
        }
    }

    protected virtual void OnPickUp(bool EnoughPlace) {
        if(eventPickUp != null) {
            eventPickUp.Invoke();
        }
    }

    protected virtual void OnUse() {
        if(eventUse != null) {
            eventUse.Invoke();
        }
    }//*/
}

public enum Action {
    None, Inspect, PickUp, Use, Receive
}

public enum InteractableType {
    None, Tab1Door, Tab1WorkerMan, Tab1Plate, Tab1UnderDeskFull, Tab1UnderDeskEmpty, Tab1SandwichBad, Tab1Batteries, Tab2Door, Tab2PictureWife, Tab2SandwichGood
}