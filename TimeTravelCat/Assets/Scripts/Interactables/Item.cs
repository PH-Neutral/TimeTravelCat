using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : Interactable {/*/
    [SerializeField] bool canBeInspected;
    [SerializeField] UnityEvent eventInspect;
    [SerializeField] bool canBePickedUp;
    [SerializeField] UnityEvent eventPickUp;
    [SerializeField] bool canBeUsed;
    [SerializeField] UnityEvent eventUse;

    protected override void Awake() {
        base.Awake();
        _canBeInspected = canBeInspected;
        _canBePickedUp = canBePickedUp;
        _canBeUsed = canBeUsed;
    }

    protected override void OnInspect() {
        eventInspect.Invoke();
    }

    protected override void OnPickUp(bool EnoughPlace) {
        eventPickUp.Invoke();
    }

    protected override void OnUse() {
        eventUse.Invoke();
    }//*/
}