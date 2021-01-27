using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public bool IsActive {
        get { return _active; }
        set {
            _active = value;
        }
    }
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public Color color;

    int NbWheelActions {
        get { return (canBeInspected ? 1 : 0) + (canBePickedUp ? 1 : 0) + (canBeUsed ? 1 : 0); }
    }
    public Action[] AvailableActions {
        get {
            List<Action> actions = new List<Action>();
            if (canBeInspected) { actions.Add(Action.Inspect); }
            if (canBePickedUp) { actions.Add(Action.PickUp); }
            if (canBeUsed) { actions.Add(Action.Use); }
            return actions.ToArray();
        }
    } 
    [SerializeField] bool canReceive = false;
    [SerializeField] protected Interactable[] receivableItems = null;
    [SerializeField] bool canBeInspected = false;
    [SerializeField] bool canBePickedUp = false;
    [SerializeField] bool canBeUsed = false;
    SpriteRenderer sRend;
    public bool wheelExists = false;
    bool empty = false;
    bool _active = true;

    private void Awake() {
        sRend = GetComponentInChildren<SpriteRenderer>();
        sprite = sRend.sprite;
        color = sRend.color;
    }

    private void OnMouseDown() {
        if (!IsActive) { return; }
        if (NbWheelActions > 1) {
            if(!wheelExists) {
                wheelExists = true;
                SpawnActionWheel();
            }
        } else if (NbWheelActions > 0) {
            OnAction(Action.Inspect);
            OnAction(Action.PickUp);
            OnAction(Action.Use);
        }
        
    }

    void SpawnActionWheel() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        ActionWheel wheel = Instantiate(GameManager.Instance.prefabActionWheel, mousePos, Quaternion.identity);
        wheel.Item = this;
    }

    void AddToInventory() {
        if (Inventory.Instance.AddItem(this)) {
            //Debug.Log("Added to Inventory!");
            sRend.enabled = false;
            empty = true;
        } else {
            Debug.Log("Failed to add to Inventory...");
        }
    }

    public void Hide(bool hide) {
        sRend.enabled = !hide;
    }

    public bool OnAction(Action action, SlotItem item = null) {
        if(!IsActive) { return false; }
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
        if(!canReceive) { return false; }
        if(!new List<Interactable>(receivableItems).Contains(item.Item)) { return false; }
        Debug.Log(name + " : OnReceive(item = " + item.Item.name + ")"); 
        item.ToInteractable(transform.position);
        Destroy(item.gameObject);
        canReceive = false;
        OnReceive(item);
        return true;
    }

    bool Inspect() {
        if(!canBeInspected) { return false; }
        Debug.Log(name + " : OnInspect()");
        OnInspect();
        return true;
    }

    bool PickUp() {
        if(!canBePickedUp) { return false; }
        bool notEnoughPlace = true;
        if(!empty) {
            AddToInventory();
            notEnoughPlace = false;
        }
        Debug.Log(name + " : OnPickUp(enoughPlace = " + !notEnoughPlace + ")");
        OnPickUp(notEnoughPlace);
        return true;
    }

    bool Use() {
        if(!canBeUsed) { return false; }
        Debug.Log(name + " : OnUse()");
        OnUse();
        return true;
    }

    protected virtual void OnReceive(SlotItem item) {
        // not implemented
    }

    protected virtual void OnInspect() {
        // not implemented
    }

    protected virtual void OnPickUp(bool notEnoughPlace) {
        // not implemented
    }

    protected virtual void OnUse() {
        // not implemented
    }
}

public enum Action {
    None, Inspect, PickUp, Use, Receive
}