using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    public Action[] AvailableActions {
        get {
            List<Action> actions = new List<Action>();
            if (eventInspect.HasBeenSet()) { actions.Add(Action.Inspect); }
            if (eventPickUp.HasBeenSet()) { actions.Add(Action.PickUp); }
            if (eventUse.HasBeenSet()) { actions.Add(Action.Use); }
            return actions.ToArray();
        }
    }
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public Color color;

    [SerializeField] Interactable receivableItem = null;
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

    public void Deactivate() {
        sRend.enabled = col.enabled = false;
    }

    public void Activate(bool activateCollider = true) {
        sRend.enabled = true;
        col.enabled = activateCollider;
    }

    public bool OnAction(Action action, SlotItem item = null) {
        //if(!IsActive) { return false; }
        switch (action) {
            case Action.Receive:
                return Receive(item);
            case Action.Inspect:
                return Inspect();
            case Action.PickUp:
                return PickUp();
            case Action.Use:
                return Use();
            default:
                break;
        }
        return false;
    }

    bool Receive(SlotItem item) {
        if(!eventReceive.HasBeenSet()) { return false; }
        if(receivableItem != item.Item) { return false; }
        Debug.Log(name + " : OnReceive(item = " + item.Item.name + ")"); 
        item.ToInteractable(transform.position);
        Destroy(item.gameObject);
        eventReceive.Invoke();
        Deactivate();
        return true;
    }

    bool Inspect() {
        if(!eventInspect.HasBeenSet()) { return false; }
        Debug.Log(name + " : OnInspect()");
        eventInspect.Invoke();
        return true;
    }

    bool PickUp() {
        if(!eventPickUp.HasBeenSet()) { return false; }
        bool EnoughPlace = Inventory.Instance.AddItem(this);
        Debug.Log(name + " : OnPickUp(enoughPlace = " + EnoughPlace + ")");
        eventPickUp.Invoke();
        return true;
    }

    bool Use() {
        if(!eventUse.HasBeenSet()) { return false; }
        Debug.Log(name + " : OnUse()");
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